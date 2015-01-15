using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents the Atbash cipher.
    /// </summary>
    public class AtbashCipher : ICipher, ISupportsCustomCharset
    {
        public AtbashCipher()
        {
            this.Charset = Charsets.English;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }

        protected IEnumerable<char> Crypt(IEnumerable<char> input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            foreach (char c in input)
            {
                int idx = this.Charset.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase);

                yield return idx != -1
                    ? (this.Charset[this.Charset.Length - idx - 1]).ToSameCaseAs(c)
                    : c
                ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public IEnumerable<char> Encrypt(IEnumerable<char> plaintext)
        {
            return this.Crypt(plaintext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public IEnumerable<char> Decrypt(IEnumerable<char> ciphertext)
        {
            return this.Crypt(ciphertext);
        }
    }
}
