using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Multiplicative cipher.
    /// </summary>
    [Export("Multiplicative", typeof(IKeyedCipher<>))]
    public class MultiplicativeCipher : IKeyedCipher<int>, ISupportsCustomCharset
    {
        public MultiplicativeCipher(string charset = Charsets.English)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            this.Charset = charset;
        }


        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }


        public string Encrypt(string plaintext, int key)
        {
            char[] result = new char[plaintext.Length];

            for (int i = 0; i < plaintext.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(plaintext[i]);

                result[i] = idx != -1
                    ? this.Charset.At(idx * key).ToSameCaseAs(plaintext[i])
                    : plaintext[i]
                ;
            }

            return new String(result);
        }

        public string Decrypt(string ciphertext, int key)
        {
            throw new NotSupportedException();
        }
    }
}
