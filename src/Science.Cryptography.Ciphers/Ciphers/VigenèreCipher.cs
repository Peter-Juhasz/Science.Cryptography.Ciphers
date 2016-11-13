using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents Blaise de Vigenère's cipher.
    /// </summary>
    [Export("Vigenère", typeof(IKeyedCipher<>))]
    public class VigenèreCipher : IKeyedCipher<string>, ISupportsCustomCharset
    {
        public VigenèreCipher(string charset = Charsets.English)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            this.Charset = charset;
            _tabulaRecta = new TabulaRecta(this.Charset);
        }

        private readonly TabulaRecta _tabulaRecta;

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }


        public string Encrypt(string plaintext, string key)
        {
            if (plaintext == null)
                throw new ArgumentNullException(nameof(plaintext));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            char[] result = new char[plaintext.Length];
            int charCounter = 0;

            for (int i = 0; i < plaintext.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(plaintext[i]);

                result[i] = idx != -1
                    ? _tabulaRecta[plaintext[i], key[charCounter++ % key.Length]].ToSameCaseAs(plaintext[i])
                    : plaintext[i]
                ;
            }

            return new String(result);
        }

        public string Decrypt(string ciphertext, string key)
        {
            if (ciphertext == null)
                throw new ArgumentNullException(nameof(ciphertext));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            char[] result = new char[ciphertext.Length];
            int charCounter = 0;

            for (int i = 0; i < ciphertext.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(ciphertext[i]);

                result[i] = idx != -1
                    ? this.Charset.At(idx - this.Charset.IndexOfIgnoreCase(key[charCounter++ % key.Length])).ToSameCaseAs(ciphertext[i])
                    : ciphertext[i]
                ;
            }

            return new String(result);
        }
    }
}
