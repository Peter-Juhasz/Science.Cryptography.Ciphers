namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents a cipher which requires a key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IKeyedCipher<TKey>
    {
        /// <summary>
        /// Encrypts <paramref name="plaintext"/> with the specified key.
        /// </summary>
        /// <param name="plaintext">The plaintext to encrypt.</param>
        /// <param name="key">The key used to encrypt.</param>
        /// <returns>The encrypted plaintext.</returns>
        string Encrypt(string plaintext, TKey key);

        /// <summary>
        /// Decrypts <paramref name="ciphertext"/> with the specified key.
        /// </summary>
        /// <param name="ciphertext">The ciphertext to decrypt.</param>
        /// <param name="key">The key used to decrypt.</param>
        /// <returns>The decrpyted ciphertext.</returns>
        string Decrypt(string ciphertext, TKey key);
    }
}
