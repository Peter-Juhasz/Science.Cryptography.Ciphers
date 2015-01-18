using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Affine cipher
    /// </summary>
    [Export("Affine", typeof(IKeyedCipher<>))]
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
