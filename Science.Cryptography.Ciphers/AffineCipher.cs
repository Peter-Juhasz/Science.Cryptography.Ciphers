using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    using Analysis;

    /// <summary>
    /// Represents the Affine cipher
    /// </summary>
    [Export("Affine", typeof(IKeyedCipher<>))]
    public class AffineCipher : IKeyedCipher<AffineKey>, ISupportsCustomCharset, IKeySpaceSource<AffineKey>
    {
        public AffineCipher()
        {
            this.Charset = Charsets.English;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }


        public string Encrypt(string plaintext, AffineKey key)
        {
            char[] ciphertext = new char[plaintext.Length];

            for (int i = 0; i < plaintext.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(plaintext[i]);

                ciphertext[i] = idx != -1
                    ? this.Charset[(key.A * idx + key.B) % this.Charset.Length].ToSameCaseAs(plaintext[i])
                    : plaintext[i]
                ;
            }

            return new String(ciphertext);
        }

        public string Decrypt(string ciphertext, AffineKey key)
        {
            char[] plaintext = new char[ciphertext.Length];

            for (int i = 0; i < ciphertext.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(ciphertext[i]);

                plaintext[i] = idx != -1
                    ? this.Charset[((this.Charset.Length - key.A) * (idx - key.B)).Mod(this.Charset.Length)].ToSameCaseAs(ciphertext[i])
                    : ciphertext[i]
                ;
            }

            return new String(plaintext);
        }


        IEnumerable<AffineKey> IKeySpaceSource<AffineKey>.GetKeys()
        {
            int n = this.Charset.Length;

            return
                from a in CoprimesTo(n)
                from b in Enumerable.Range(0, n)
                select new AffineKey(a, b)
            ;
        }

        private static int Gcd(int a, int b)
        {
            int rem;

            while (b != 0)
            {
                rem = a % b;
                a = b;
                b = rem;
            }

            return a;
        }

        private static IEnumerable<int> CoprimesTo(int n)
        {
            return Enumerable.Range(0, n)
                .Where(i => Gcd(i, n) == 1)
            ;
        }
    }

    /// <summary>
    /// Represents a key for the <see cref="AffineCipher" />.
    /// </summary>
    public struct AffineKey : IEquatable<AffineKey>
    {
        public AffineKey(int a, int b)
        {
            _A = a;
            _B = b;
        }


        private int _A;
        /// <summary>
        /// Gets or sets the first part of the key.
        /// </summary>
        public int A
        {
            get { return _A; }
            set { _A = value; }
        }

        private int _B;
        /// <summary>
        /// Gets or sets the second part of the key.
        /// </summary>
        public int B
        {
            get { return _B; }
            set { _B = value; }
        }


        public bool Equals(AffineKey other)
        {
            return this.A == other.A && this.B == other.B;
        }
    }
}
