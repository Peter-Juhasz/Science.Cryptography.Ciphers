using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents the Affine cipher.
    /// </summary>
    [Export("Affine", typeof(IKeyedCipher<>))]
    public class AffineCipher : IKeyedCipher<AffineKey>, ISupportsCustomCharset
    {
        public AffineCipher(string charset = Charsets.English)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            this.Charset = charset;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }

        public IEnumerable<char> Encrypt(IEnumerable<char> plaintext, AffineKey key)
        {
            return
                from c in plaintext
                let idx = this.Charset.IndexOfIgnoreCase(c)
                select idx != -1
                    ? this.Charset[(key.A * idx + key.B) % this.Charset.Length].ToSameCaseAs(c)
                    : c
            ;
        }

        public IEnumerable<char> Decrypt(IEnumerable<char> ciphertext, AffineKey key)
        {
            return
                from c in ciphertext
                let idx = this.Charset.IndexOfIgnoreCase(c)
                select idx != -1
                    ? this.Charset.At((this.Charset.Length - key.A) * (idx - key.B)).ToSameCaseAs(c)
                    : c
            ;
        }
    }
}
