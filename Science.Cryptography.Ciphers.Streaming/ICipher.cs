using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents a cipher which works with streams of characters.
    /// </summary>
    public interface ICipher
    {
        /// <summary>
        /// Encrypts the input stream of characters.
        /// </summary>
        /// <param name="decrypted">Input stream of characters.</param>
        /// <returns></returns>
        IEnumerable<char> Encrypt(IEnumerable<char> decrypted);

        /// <summary>
        /// Decrypts the input stream of characters.
        /// </summary>
        /// <param name="encrypted">Input stream of characters.</param>
        /// <returns></returns>
        IEnumerable<char> Decrypt(IEnumerable<char> encrypted);
    }
}
