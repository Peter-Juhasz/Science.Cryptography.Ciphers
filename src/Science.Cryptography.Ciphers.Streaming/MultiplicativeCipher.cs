using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

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
            return
                from c in plaintext
                let idx = this.Charset.IndexOfIgnoreCase(c)
                select idx != -1
                    ? this.Charset.At(idx * key).ToSameCaseAs(c)
                    : c
            ;
        }

        public IEnumerable<char> Decrypt(IEnumerable<char> ciphertext, int key)
        {
            throw new NotSupportedException();
        }
    }
}
