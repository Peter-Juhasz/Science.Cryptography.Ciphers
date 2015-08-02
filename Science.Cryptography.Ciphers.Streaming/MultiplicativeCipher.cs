using System;
using System.Collections.Generic;
using System.Composition;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents the Multiplicative cipher.
    /// </summary>
    [Export("Multiplicative", typeof(IKeyedCipher<>))]
    public class MultiplicativeCipher : IKeyedCipher<int>
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


        public IEnumerable<char> Encrypt(IEnumerable<char> plaintext, int key)
        {
            foreach (char c in plaintext)
            {
                int idx = this.Charset.IndexOfIgnoreCase(c);

                yield return idx != -1
                    ? this.Charset[(idx * key).Mod(this.Charset.Length)].ToSameCaseAs(c)
                    : c
                ;
            }
        }

        public IEnumerable<char> Decrypt(IEnumerable<char> ciphertext, int key)
        {
            throw new NotSupportedException();
        }
    }
}
