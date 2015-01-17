using System;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents Blaise de Vigenère's cipher.
    /// </summary>
    public class VigenèreCipher : IKeyedCipher<string>, ISupportsCustomCharset
    {
        public VigenèreCipher()
        {
            this.Charset = Charsets.English;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }


        public string Encrypt(string plaintext, string key)
        {
            char[] result = new char[plaintext.Length];
            int charCounter = 0;

            for (int i = 0; i < plaintext.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(plaintext[i]);

                result[i] = idx != -1
                    ? this.Charset[(idx + this.Charset.IndexOfIgnoreCase(key[charCounter++ % key.Length])).Mod(this.Charset.Length)].ToSameCaseAs(plaintext[i])
                    : plaintext[i]
                ;
            }

            return new String(result);
        }

        public string Decrypt(string ciphertext, string key)
        {
            char[] result = new char[ciphertext.Length];
            int charCounter = 0;

            for (int i = 0; i < ciphertext.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(ciphertext[i]);

                result[i] = idx != -1
                    ? this.Charset[(idx - this.Charset.IndexOfIgnoreCase(key[charCounter++ % key.Length])).Mod(this.Charset.Length)].ToSameCaseAs(ciphertext[i])
                    : ciphertext[i]
                ;
            }

            return new String(result);
        }
    }
}
