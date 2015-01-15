using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents the Affine cipher.
    /// </summary>
    public class AffineCipher : IKeyedCipher<AffineKey>, ISupportsCustomCharset
    {
        public AffineCipher()
        {
            this.Charset = Charsets.English;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }

        public IEnumerable<char> Encrypt(IEnumerable<char> plaintext, AffineKey key)
        {
            foreach (char c in plaintext)
            {
                int idx = this.Charset.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase);

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
                int idx = this.Charset.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase);

                yield return idx != -1
                    ? this.Charset[Mod((this.Charset.Length - key.A) * (idx - key.B), this.Charset.Length)].ToSameCaseAs(c)
                    : c
                ;
            }
        }

        internal int Mod(int a, int b)
        {
            return a >= 0 ? a % b : (b + a) % b;
        }
    }
}
