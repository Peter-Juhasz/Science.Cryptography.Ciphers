using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Provides keys for cracking.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public interface IKeySpace<TKey>
{
	/// <summary>
	/// Gets the keys of the key space for ciphertext analysis.
	/// </summary>
	/// <returns></returns>
	IEnumerable<TKey> GetKeys();
}
