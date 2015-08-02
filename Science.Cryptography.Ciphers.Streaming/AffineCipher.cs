﻿using System;
using System.Collections.Generic;
using System.Composition;

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
            foreach (char c in plaintext)
            {
                int idx = this.Charset.IndexOfIgnoreCase(c);

                yield return idx != -1
                    ? this.Charset[(key.A * idx + key.B) % this.Charset.Length].ToSameCaseAs(c)
                    : c
                ;
            }
        }

        public IEnumerable<char> Decrypt(IEnumerable<char> ciphertext, AffineKey key)
        {
            foreach (char c in ciphertext)
            {
                int idx = this.Charset.IndexOfIgnoreCase(c);

                yield return idx != -1
                    ? this.Charset[((this.Charset.Length - key.A) * (idx - key.B)).Mod(this.Charset.Length)].ToSameCaseAs(c)
                    : c
                ;
            }
        }
    }
}
