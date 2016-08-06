using System;

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

    public static class KeyedCipherExtensions
    {
        /// <summary>
        /// Pins a key to a <see cref="IKeyedCipher{TKey}"/> and converts it to a <see cref="ICipher"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key of <paramref name="keyedCipher"/>.</typeparam>
        /// <param name="keyedCipher">The cipher to use.</param>
        /// <param name="key">The key to use.</param>
        /// <returns>A <see cref="ICipher"/> which operates with the provided fix key.</returns>
        public static ICipher Pin<TKey>(this IKeyedCipher<TKey> keyedCipher, TKey key)
        {
            if (keyedCipher == null)
                throw new ArgumentNullException(nameof(keyedCipher));
            
            return new PinnedKeyCipher<TKey>(keyedCipher, key);
        }

        private sealed class PinnedKeyCipher<TKey> : ICipher
        {
            public PinnedKeyCipher(IKeyedCipher<TKey> keyedCipher, TKey key)
            {
                if (keyedCipher == null)
                    throw new ArgumentNullException(nameof(keyedCipher));

                this.Cipher = keyedCipher;
                this.Key = key;
            }

            public IKeyedCipher<TKey> Cipher { get; private set; }

            public TKey Key { get; private set; }

            public string Encrypt(string plaintext)
            {
                return this.Cipher.Encrypt(plaintext, this.Key);
            }

            public string Decrypt(string ciphertext)
            {
                return this.Cipher.Decrypt(ciphertext, this.Key);
            }
        }
    }
}
