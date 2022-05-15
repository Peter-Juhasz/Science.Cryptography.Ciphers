using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Science.Cryptography.Ciphers.Analysis.Tests")]
[assembly: InternalsVisibleTo("Science.Cryptography.Ciphers.Benchmarks")]

namespace Science.Cryptography.Ciphers.Analysis;

public class CryptogramSolver
{
	internal CryptogramSolver(CryptogramSolverBuilder options)
	{
		Options = options;
		Logger = options.Logger ?? NullLogger.Instance;
	}

	private CryptogramSolverBuilder Options { get; }
	private ILogger Logger { get; }

	private static readonly MethodInfo keyFinderSolveMethod = typeof(KeyFinder).GetMethod(nameof(KeyFinder.Solve))!;
	private static readonly MethodInfo keyFinderSolveAsyncMethod = typeof(KeyFinder).GetMethod(nameof(KeyFinder.SolveAsync))!;
	private static readonly MethodInfo keyFinderSolveParallelMethod = typeof(KeyFinder).GetMethods(BindingFlags.Public | BindingFlags.Static)!.Where(m => m.Name == nameof(KeyFinder.SolveParallelAsync)).Single(m => m.GetParameters()[2].ParameterType.GetGenericTypeDefinition() == typeof(IKeySpace<>));
	private static readonly MethodInfo keyFinderSolveParallelPartitionedMethod = typeof(KeyFinder).GetMethods(BindingFlags.Public | BindingFlags.Static)!.Where(m => m.Name == nameof(KeyFinder.SolveParallelAsync)).Single(m => m.GetParameters()[2].ParameterType.GetGenericTypeDefinition() == typeof(IPartitionedKeySpace<>));
	private static readonly MethodInfo keyFinderSolveParallelAsyncMethod = typeof(KeyFinder).GetMethods(BindingFlags.Public | BindingFlags.Static)!.Where(m => m.Name == nameof(KeyFinder.SolveParallelAsync)).Single(m => m.GetParameters()[2].ParameterType.GetGenericTypeDefinition() == typeof(IAsyncKeySpace<>));

	private static readonly MethodInfo copyToSyncMethod = typeof(CryptogramSolver).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)!.Where(m => m.Name == nameof(CopyToAsync)).Single(m => m.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>));
	private static readonly MethodInfo copyToAsyncMethod = typeof(CryptogramSolver).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)!.Where(m => m.Name == nameof(CopyToAsync)).Single(m => m.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>));

	public IAsyncEnumerable<SolverResult> SolveAsync(string ciphertext, ICandidatePromoter? candidatePromoter = null, CancellationToken cancellationToken = default)
	{
		candidatePromoter ??= new ProgressivelyBetterCandidatePromoter();

		var channel = Channel.CreateUnbounded<SolverResult>();

		Task.Run(() => RunAsync(channel.Writer, ciphertext, candidatePromoter, cancellationToken));

		return channel.Reader.ReadAllAsync(cancellationToken);
	}

	private async Task RunAsync(ChannelWriter<SolverResult> channel, string ciphertext, ICandidatePromoter candidatePromoter, CancellationToken cancellationToken)
	{
		try
		{
			Logger.LogInformation("Analysis started.");
			var buffer = new char[ciphertext.Length];
			int written;

			var localScorer = Options.Scorer switch
			{
				IPartitionedSpeculativePlaintextScorer partitioned => partitioned.GetForPartition(),
				ISpeculativePlaintextScorer scorer => scorer
			};

			// unkeyed ciphers
			foreach (var cipher in Options.Ciphers)
			{
				cancellationToken.ThrowIfCancellationRequested();
				Logger.LogInformation("Running cipher '{cipher}'...", cipher.GetType().Name);

				// ensure buffer
				var desiredSize = cipher.MaxOutputCharactersPerInputCharacter * ciphertext.Length;
				if (desiredSize > buffer.Length)
				{
					Logger.LogDebug("Resizing buffer from {from} bytes to {to} bytes.", buffer.Length, desiredSize);
					Array.Resize(ref buffer, desiredSize);
				}

				// run cipher
				try
				{
					cipher.Decrypt(ciphertext, buffer, out written);
				}
				catch (Exception ex)
				{
					Logger.LogWarning(ex, "An error occurred while running cipher '{cipher}'.", cipher.GetType().Name);
					continue;
				}

				if (written == 0)
				{
					continue;
				}

				// evaluate
				var score = localScorer.Score(buffer.AsSpan(..written));
				if (candidatePromoter.Promote(score))
				{
					await channel.WriteAsync(new(new(new string(buffer.AsSpan(..written)), score, localScorer), new(cipher)), cancellationToken);
				}
			}

			// keyed ciphers
			foreach (var (keyedCipher, keySpace) in Options.KeyedCiphers)
			{
				cancellationToken.ThrowIfCancellationRequested();
				Logger.LogInformation("Running cipher '{cipher}' with key space '{source}'...", keyedCipher.GetType().Name, keySpace.GetType().Name);

				var keyType = keyedCipher.GetType().GetInterface("IKeyedCipher`1")!.GenericTypeArguments[0]!;

				var isPartitionedKeySpace = keySpace.GetType().GetInterface("IPartitionedKeySpace`1") is not null;
				var isAsyncKeySpace = keySpace.GetType().GetInterface("IAsyncKeySpace`1") is not null;
				var useParallel = false;
				var method = isPartitionedKeySpace switch
				{
					true => keyFinderSolveParallelPartitionedMethod,
					false => (isAsyncKeySpace, useParallel) switch
					{
						(true, false) => keyFinderSolveAsyncMethod,
						(false, false) => keyFinderSolveMethod,
						(true, true) => keyFinderSolveParallelAsyncMethod,
						(false, true) => keyFinderSolveParallelMethod,
					}
				};

				try
				{
					// solve
					var enumerable = method.MakeGenericMethod(keyType).Invoke(null, new[] { ciphertext, keyedCipher, keySpace, Options.Scorer, candidatePromoter, Logger, cancellationToken })!;
					var copyMethod = (isAsyncKeySpace || isPartitionedKeySpace) switch
					{
						true => copyToAsyncMethod,
						false => copyToSyncMethod,
					};
					await (Task)copyMethod.MakeGenericMethod(keyType).Invoke(null, new[] { keyedCipher, enumerable, channel, cancellationToken })!;
				}
				catch (Exception ex)
				{
					Logger.LogWarning(ex, "An error occurred while running cipher '{cipher}'.", keyedCipher.GetType().Name);
					continue;
				}
			}

			channel.Complete();
		}
		catch (OperationCanceledException)
		{
			Logger.LogInformation("Operation cancelled.");
			channel.Complete();
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "An error occurred during analysis.");
			channel.Complete(ex);
		}
		finally
		{
			Logger.LogInformation("Analysis finished.");
		}
	}

	private static async Task CopyToAsync<TKey>(IKeyedCipher<TKey> cipher, IEnumerable<KeyFinderResult<TKey>> finderResults, ChannelWriter<SolverResult> finalResults, CancellationToken cancellationToken)
	{
		foreach (var result in finderResults)
		{
			await finalResults.WriteAsync(new(result.SpeculativePlaintext, new(cipher, result.Key!)), cancellationToken);
		}
	}

	private static async Task CopyToAsync<TKey>(IKeyedCipher<TKey> cipher, IAsyncEnumerable<KeyFinderResult<TKey>> finderResults, ChannelWriter<SolverResult> finalResults, CancellationToken cancellationToken)
	{
		await foreach (var result in finderResults)
		{
			await finalResults.WriteAsync(new(result.SpeculativePlaintext, new(cipher, result.Key!)), cancellationToken);
		}
	}
}
