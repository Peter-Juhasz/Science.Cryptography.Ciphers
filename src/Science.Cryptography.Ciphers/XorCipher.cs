using Science.Cryptography.Ciphers;
using System;
using System.Composition;
using System.Linq;

namespace ClassLibrary2
{
    /// <summary>
    /// Represents the XOR cipher.
    /// </summary>
    [Export("XOR", typeof(IKeyedCipher<>))]
    public class XorCipher : IKeyedCipher<byte[]>
    {
        public string Encrypt(string plaintext, byte[] key)
        {
            return Crypt(plaintext, key);
        }

        public string Decrypt(string ciphertext, byte[] key)
        {
            return Crypt(ciphertext, key);
        }


        internal static string Crypt(string text, byte[] key)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (key.Length == 0)
                throw new ArgumentException("Key can't be zero-length.", nameof(key));

            return String.Concat(
                text.Zip(EnumerableEx.Repeat<byte>(key), (c, k) => (char)(c ^ k))
            );
        }
    }
}
