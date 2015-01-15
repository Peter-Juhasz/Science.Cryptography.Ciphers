using System;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Affine cipher
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

        public string Encrypt(string plaintext, AffineKey key)
        {
            char[] ciphertext = new char[plaintext.Length];

            for (int i = 0; i < plaintext.Length; i++)
            {
                int idx = this.Charset.IndexOf(plaintext[i].ToString(), StringComparison.OrdinalIgnoreCase);

                if (idx != -1)
                    ciphertext[i] = this.Charset[(key.A * idx + key.B) % this.Charset.Length].ToSameCaseAs(plaintext[i]);
                else
                    ciphertext[i] = plaintext[i];
            }

            return new String(ciphertext);
        }

        public string Decrypt(string ciphertext, AffineKey key)
        {
            char[] plaintext = new char[ciphertext.Length];

            for (int i = 0; i < ciphertext.Length; i++)
            {
                int idx = this.Charset.IndexOf(ciphertext[i].ToString(), StringComparison.OrdinalIgnoreCase);

                if (idx != -1)
                    plaintext[i] = this.Charset[Mod((this.Charset.Length - key.A) * (idx - key.B), this.Charset.Length)].ToSameCaseAs(ciphertext[i]);
                else
                    plaintext[i] = ciphertext[i];
            }

            return new String(plaintext);
        }

        internal int Mod(int a, int b)
        {
            return a >= 0 ? a % b : (b + a) % b;
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
