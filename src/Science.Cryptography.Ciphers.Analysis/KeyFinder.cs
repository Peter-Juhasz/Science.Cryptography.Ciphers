using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Science.Cryptography.Ciphers.Analysis;

public static class KeyFinder
{
	public static IEnumerable<KeyFinderResult<TKey>> Solve<TKey>(
		string ciphertext,
		IKeyedCipher<TKey> cipher,
		IKeySpace<TKey> keySpace,
		ISpeculativePlaintextScorer scorer,
		ICandidatePromoter? candidatePromoter = null,
		ILogger? logger = null,
		CancellationToken cancellationToken = default
	)
	{
		// initialize
		var buffer = new char[ciphertext.Length];
		int written;
		candidatePromoter ??= new ProgressivelyBetterCandidatePromoter();
		logger ??= NullLogger.Instance;
		if (scorer is IPartitionedSpeculativePlaintextScorer partitioned) scorer = partitioned.GetForPartition();

		// enumerate key space
		foreach (var key in keySpace.GetKeys())
		{
			cancellationToken.ThrowIfCancellationRequested();

			// ensure buffer
			var desiredSize = ciphertext.Length * cipher.GetMaxOutputCharactersPerInputCharacter(key);
			if (desiredSize > buffer.Length)
			{
				logger.LogDebug("Resizing buffer from {from} bytes to {to} bytes.", buffer.Length, desiredSize);
				Array.Resize(ref buffer, desiredSize);
			}

			// run cipher
			try
			{
				cipher.Decrypt(ciphertext, buffer, key, out written);
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "An error occurred while running cipher '{cipher}'.", cipher.GetType().Name);
				continue;
			}

			if (written == 0)
			{
				continue;
			}

			// evaluate
			var score = scorer.Score(buffer.AsSpan(..written));
			if (candidatePromoter.Promote(score))
			{
				yield return new(key, new(new string(buffer.AsSpan()[..written]), score, scorer));
			}
		}
	}

	public static async IAsyncEnumerable<KeyFinderResult<TKey>> SolveAsync<TKey>(
		string ciphertext,
		IKeyedCipher<TKey> cipher,
		IAsyncKeySpace<TKey> keySpace,
		ISpeculativePlaintextScorer scorer,
		ICandidatePromoter? candidatePromoter = null,
		ILogger? logger = null,
		[EnumeratorCancellation] CancellationToken cancellationToken = default
	)
	{
		// initialize
		var buffer = new char[ciphertext.Length];
		int written;
		candidatePromoter ??= new ProgressivelyBetterCandidatePromoter();
		logger ??= NullLogger.Instance;

		// enumerate key space
		await foreach (var key in keySpace.GetKeysAsync(cancellationToken))
		{
			cancellationToken.ThrowIfCancellationRequested();

			// ensure buffer
			var desiredSize = ciphertext.Length * cipher.GetMaxOutputCharactersPerInputCharacter(key);
			if (desiredSize > buffer.Length)
			{
				logger.LogDebug("Resizing buffer from {from} bytes to {to} bytes.", buffer.Length, desiredSize);
				Array.Resize(ref buffer, desiredSize);
			}

			// run cipher
			try
			{
				cipher.Decrypt(ciphertext, buffer, key, out written);
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex, "An error occurred while running cipher '{cipher}'.", cipher.GetType().Name);
				continue;
			}

			if (written == 0)
			{
				continue;
			}

			// evaluate
			var score = scorer.Score(buffer.AsSpan(..written));
			if (candidatePromoter.Promote(score))
			{
				yield return new(key, new(new string(buffer.AsSpan()[..written]), score, scorer));
			}
		}
	}

	public static IAsyncEnumerable<KeyFinderResult<TKey>> SolveParallelAsync<TKey>(
		string ciphertext,
		IKeyedCipher<TKey> cipher,
		IKeySpace<TKey> keySpace,
		ISpeculativePlaintextScorer scorer,
		ICandidatePromoter? candidatePromoter = null,
		ILogger? logger = null,
		CancellationToken cancellationToken = default
	)
	{
		candidatePromoter ??= new ProgressivelyBetterCandidatePromoter();
		logger ??= NullLogger.Instance;
		var degreeOfParallelism = Environment.ProcessorCount;
		var resultsChannel = Channel.CreateUnbounded<KeyFinderResult<TKey>>(new UnboundedChannelOptions()
		{
			SingleWriter = false,
		});
		var keysChannel = Channel.CreateBounded<TKey>(new BoundedChannelOptions(degreeOfParallelism)
		{
			SingleWriter = true,
			SingleReader = false,
		});

		// key publisher
		var keyTask = Task.Run(() => PublishKeys(keySpace, keysChannel.Writer, logger, cancellationToken));

		// workers
		Task[] tasks = new Task[degreeOfParallelism];
		for (int i = 0; i < degreeOfParallelism; i++)
		{
			tasks[i] = Task.Run(() => ProcessAsync(ciphertext, keysChannel.Reader, cipher, scorer, candidatePromoter, resultsChannel.Writer, logger, cancellationToken));
		}

		Task.Run(async () =>
		{
			var completer = resultsChannel.Writer;
			try
			{
				await Task.WhenAll(Task.WhenAll(tasks), keyTask);
				completer.Complete();
			}
			catch (OperationCanceledException)
			{
				completer.Complete();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occurred while running parallel analysis.");
				completer.Complete(ex);
			}
		});

		return resultsChannel.Reader.ReadAllAsync(cancellationToken);
	}

	public static IAsyncEnumerable<KeyFinderResult<TKey>> SolveParallelAsync<TKey>(
		string ciphertext,
		IKeyedCipher<TKey> cipher,
		IPartitionedKeySpace<TKey> keySpace,
		ISpeculativePlaintextScorer scorer,
		ICandidatePromoter? candidatePromoter = null,
		ILogger? logger = null,
		CancellationToken cancellationToken = default
	)
	{
		candidatePromoter ??= new ProgressivelyBetterCandidatePromoter();
		logger ??= NullLogger.Instance;
		var degreeOfParallelism = Environment.ProcessorCount;
		var resultsChannel = Channel.CreateUnbounded<KeyFinderResult<TKey>>(new UnboundedChannelOptions()
		{
			SingleWriter = false,
		});
		var keysChannel = Channel.CreateBounded<TKey>(new BoundedChannelOptions(degreeOfParallelism)
		{
			SingleWriter = true,
			SingleReader = false,
		});

		// keys
		var partitions = keySpace.GetPartitions(desiredCount: degreeOfParallelism);

		// workers
		var options = new ParallelOptions() 
		{
			MaxDegreeOfParallelism = degreeOfParallelism,
			CancellationToken = cancellationToken,
		};
		Task.Run(async () =>
		{
			var completer = resultsChannel.Writer;
			try
			{
				await Parallel.ForEachAsync(partitions, options, (partition, cancellationToken) => ProcessPartitionAsync(ciphertext, partition, cipher, scorer, candidatePromoter, resultsChannel.Writer, logger, cancellationToken));
				completer.Complete();
			}
			catch (OperationCanceledException)
			{
				completer.Complete();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occurred while running parallel analysis.");
				completer.Complete(ex);
			}
		});

		return resultsChannel.Reader.ReadAllAsync(cancellationToken);
	}

	public static IAsyncEnumerable<KeyFinderResult<TKey>> SolveParallelAsync<TKey>(
		string ciphertext,
		IKeyedCipher<TKey> cipher,
		IAsyncKeySpace<TKey> keySpace,
		ISpeculativePlaintextScorer scorer,
		ICandidatePromoter? candidatePromoter = null,
		ILogger? logger = null,
		CancellationToken cancellationToken = default
	)
	{
		candidatePromoter ??= new ProgressivelyBetterCandidatePromoter();
		logger ??= NullLogger.Instance;
		var degreeOfParallelism = Environment.ProcessorCount;
		var resultsChannel = Channel.CreateUnbounded<KeyFinderResult<TKey>>(new UnboundedChannelOptions()
		{
			SingleWriter = false,
		});
		var keysChannel = Channel.CreateBounded<TKey>(new BoundedChannelOptions(degreeOfParallelism)
		{
			SingleWriter = true,
			SingleReader = false,
		});

		// key publisher
		var keyTask = Task.Run(() => PublishKeys(keySpace, keysChannel.Writer, logger, cancellationToken));

		// workers
		Task[] tasks = new Task[degreeOfParallelism];
		for (int i = 0; i < degreeOfParallelism; i++)
		{
			tasks[i] = Task.Run(() => ProcessAsync(ciphertext, keysChannel.Reader, cipher, scorer, candidatePromoter, resultsChannel.Writer, logger, cancellationToken));
		}
		Task.Run(async () =>
		{
			var completer = resultsChannel.Writer;
			try
			{
				await Task.WhenAll(Task.WhenAll(tasks), keyTask);
				completer.Complete();
			}
			catch (OperationCanceledException)
			{
				completer.Complete();
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "An error occurred while running parallel analysis.");
				completer.Complete(ex);
			}
		});
		
		return resultsChannel.Reader.ReadAllAsync(cancellationToken);
	}

	private static async Task ProcessAsync<TKey>(
		string ciphertext,
		ChannelReader<TKey> keyReader, 
		IKeyedCipher<TKey> cipher,
		ISpeculativePlaintextScorer scorer,
		ICandidatePromoter candidatePromoter,
		ChannelWriter<KeyFinderResult<TKey>> resultWriter,
		ILogger logger, 
		CancellationToken cancellationToken
	)
	{
		try
		{
			// initialize
			var buffer = new char[ciphertext.Length];
			int written;
			if (scorer is IPartitionedSpeculativePlaintextScorer partitioned) scorer = partitioned.GetForPartition();

			await foreach (var key in keyReader.ReadAllAsync(cancellationToken))
			{
				cancellationToken.ThrowIfCancellationRequested();

				// ensure buffer
				var desiredSize = ciphertext.Length * cipher.GetMaxOutputCharactersPerInputCharacter(key);
				if (desiredSize > buffer.Length)
				{
					logger.LogDebug("Resizing buffer from {from} bytes to {to} bytes.", buffer.Length, desiredSize);
					Array.Resize(ref buffer, desiredSize);
				}

				// run cipher
				cipher.Decrypt(ciphertext, buffer, key, out written);

				if (written == 0)
				{
					continue;
				}

				// evaluate
				var score = scorer.Score(buffer.AsSpan(..written));
				if (candidatePromoter.Promote(score))
				{
					await resultWriter.WriteAsync(new(key, new(new string(buffer.AsSpan()[..written]), score, scorer)), cancellationToken);
				}
			}
		}
		catch (OperationCanceledException)
		{
			logger.LogDebug("Operation cancelled.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while running cipher.");
		}
	}

	private static async ValueTask ProcessPartitionAsync<TKey>(
		string ciphertext,
		IKeySpace<TKey> partition,
		IKeyedCipher<TKey> cipher,
		ISpeculativePlaintextScorer scorer,
		ICandidatePromoter candidatePromoter,
		ChannelWriter<KeyFinderResult<TKey>> resultWriter,
		ILogger logger,
		CancellationToken cancellationToken
	)
	{
		try
		{
			// initialize
			var buffer = new char[ciphertext.Length];
			int written;
			if (scorer is IPartitionedSpeculativePlaintextScorer partitioned) scorer = partitioned.GetForPartition();

			foreach (var key in partition.GetKeys())
			{
				cancellationToken.ThrowIfCancellationRequested();

				// ensure buffer
				var desiredSize = ciphertext.Length * cipher.GetMaxOutputCharactersPerInputCharacter(key);
				if (desiredSize > buffer.Length)
				{
					logger.LogDebug("Resizing buffer from {from} bytes to {to} bytes.", buffer.Length, desiredSize);
					Array.Resize(ref buffer, desiredSize);
				}

				// run cipher
				cipher.Decrypt(ciphertext, buffer, key, out written);

				if (written == 0)
				{
					continue;
				}

				// evaluate
				var score = scorer.Score(buffer.AsSpan(..written));
				if (candidatePromoter.Promote(score))
				{
					await resultWriter.WriteAsync(new(key, new(new string(buffer.AsSpan()[..written]), score, scorer)), cancellationToken);
				}
			}
		}
		catch (OperationCanceledException)
		{
			logger.LogDebug("Operation cancelled.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while running cipher.");
		}
	}

	private static async Task PublishKeys<TKey>(IKeySpace<TKey> keySpace, ChannelWriter<TKey> writer, ILogger logger, CancellationToken cancellationToken)
	{
		try
		{
			foreach (var key in keySpace.GetKeys())
			{
				cancellationToken.ThrowIfCancellationRequested();

				await writer.WriteAsync(key, cancellationToken);
			}
		}
		catch (OperationCanceledException)
		{
			logger.LogDebug("Operation cancelled.");
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while enumerating key space.");
		}
	}

	private static async Task PublishKeys<TKey>(IAsyncKeySpace<TKey> keySpace, ChannelWriter<TKey> writer, ILogger logger, CancellationToken cancellationToken)
	{
		try
		{
			await foreach (var key in keySpace.GetKeysAsync(cancellationToken))
			{
				await writer.WriteAsync(key, cancellationToken);
			}

			writer.Complete();
		}
		catch (OperationCanceledException)
		{
			logger.LogDebug("Operation cancelled.");
			writer.Complete();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "An error occurred while enumerating key space.");
			writer.Complete(ex);
		}
	}


	public static KeyFinderResult<TKey>? SolveForBest<TKey>(
		string ciphertext,
		IKeyedCipher<TKey> cipher,
		IKeySpace<TKey> keySpace,
		ISpeculativePlaintextScorer scorer,
		ILogger? logger = null,
		CancellationToken cancellationToken = default
	) =>
		Solve(ciphertext, cipher, keySpace, scorer, new ProgressivelyBetterCandidatePromoter(), logger, cancellationToken).LastOrDefault();
}
