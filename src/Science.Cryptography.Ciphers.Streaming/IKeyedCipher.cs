using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents a cipher which works with streams of characters and requires a key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IKeyedCipher<TKey>
    {
        /// <summary>
        /// Encrypts the input stream of characters with a specified key.
        /// </summary>
        /// <param name="decrypted">Input stream of characters.</param>
        /// <returns></returns>
        IEnumerable<char> Encrypt(IEnumerable<char> input, TKey key);

        /// <summary>
        /// Decrypts the input stream of characters with a specified key.
        /// </summary>
        /// <param name="encrypted">Input stream of characters.</param>
        /// <returns></returns>
        IEnumerable<char> Decrypt(IEnumerable<char> input, TKey key);
    }
}
