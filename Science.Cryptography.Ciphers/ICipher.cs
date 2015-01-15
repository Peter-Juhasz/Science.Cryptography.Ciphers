namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents a cipher.
    /// </summary>
    public interface ICipher
    {
        /// <summary>
        /// Encrypts <paramref name="plaintext"/>.
        /// </summary>
        /// <param name="plaintext">The plaintext to encrypt.</param>
        /// <returns>The encrypted plaintext.</returns>
        string Encrypt(string plaintext);

        /// <summary>
        /// Decrypts <paramref name="ciphertext"/>.
        /// </summary>
        /// <param name="ciphertext">The ciphertext to decrypt.</param>
        /// <returns>The decrpyted ciphertext.</returns>
        string Decrypt(string ciphertext);
    }
}
