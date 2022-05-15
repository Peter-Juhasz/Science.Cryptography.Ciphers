using Microsoft.Extensions.Logging;
using Science.Cryptography.Ciphers.Analysis.KeySpace;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

public class CryptogramSolverBuilder
{
	public CryptogramSolverBuilder(ISpeculativePlaintextScorer scorer)
	{
		Scorer = scorer;
	}

	internal IList<ICipher> Ciphers { get; } = new List<ICipher>();
	internal IList<(object cipher, object source)> KeyedCiphers { get; } = new List<(object cipher, object source)>();

	public ISpeculativePlaintextScorer Scorer { get; private set; }

	public ILogger? Logger { get; private set; }

	public CryptogramSolverBuilder AddCipher(ICipher cipher)
	{
		Ciphers.Add(cipher);
		return this;
	}

	public CryptogramSolverBuilder AddCipher<TKey>(IKeyedCipher<TKey> cipher, IKeySpace<TKey> source)
	{
		KeyedCiphers.Add((cipher, source));
		return this;
	}

	public CryptogramSolverBuilder AddCipher(IKeyedCipher<string> cipher, IKeySpace<char[]> source)
	{
		KeyedCiphers.Add((cipher, source.AsString()));
		return this;
	}

	public CryptogramSolverBuilder AddCipher(IKeyedCipher<string> cipher, IPartitionedKeySpace<char[]> source)
	{
		KeyedCiphers.Add((cipher, source.AsString()));
		return this;
	}

	public CryptogramSolverBuilder AddCipher(IKeyedCipher<string> cipher, IAsyncKeySpace<char[]> source)
	{
		KeyedCiphers.Add((cipher, source.AsString()));
		return this;
	}

	public CryptogramSolverBuilder AddCipher<TKey>(IKeyedCipher<TKey> cipher, IPartitionedKeySpace<TKey> source)
	{
		KeyedCiphers.Add((cipher, source));
		return this;
	}

	public CryptogramSolverBuilder AddCipher<TKey>(IKeyedCipher<TKey> cipher, IAsyncKeySpace<TKey> source)
	{
		KeyedCiphers.Add((cipher, source));
		return this;
	}

	public CryptogramSolverBuilder SetScorer(ISpeculativePlaintextScorer scorer)
	{
		Scorer = scorer;
		return this;
	}

	public CryptogramSolverBuilder SetLogger(ILogger logger)
	{
		Logger = logger;
		return this;
	}

	public CryptogramSolver Build() => new(this);
}
