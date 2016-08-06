using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents the Atbash cipher.
    /// </summary>
    [Export("Atbash", typeof(ICipher))]
    public class AtbashCipher : ICipher, ISupportsCustomCharset
    {
        public AtbashCipher(string charset = Charsets.English)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            this.Charset = charset;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }

        protected IEnumerable<char> Crypt(IEnumerable<char> input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return
                from c in input
                let idx = this.Charset.IndexOfIgnoreCase(c)
                select idx != -1
                    ? this.Charset[this.Charset.Length - idx - 1].ToSameCaseAs(c)
                    : c
            ;
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
