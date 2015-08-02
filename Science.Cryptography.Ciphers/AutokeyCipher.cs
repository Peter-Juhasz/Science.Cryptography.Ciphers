using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Autokey cipher.
    /// </summary>
    [Export("Autokey", typeof(IKeyedCipher<>))]
    public class AutokeyCipher : IKeyedCipher<string>, ISupportsCustomCharset
    {
        public AutokeyCipher(string charset = Charsets.English)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            this.Charset = charset;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }

        public string Encrypt(string plaintext, string key)
        {
            string autokey = key + plaintext;

            char[] result = new char[plaintext.Length];
            int charCounter = 0;

            autokey = autokey.ToUpper();

            for (int i = 0; i < plaintext.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(plaintext[i]);

                result[i] = idx != -1
                    ? this.Charset[(idx + this.Charset.IndexOf(autokey[charCounter++])) % this.Charset.Length].ToSameCaseAs(plaintext[i])
                    : plaintext[i]
                ;
            }

            return new String(result);
        }

        public string Decrypt(string ciphertext, string key)
        {
            char[] result = new char[ciphertext.Length];

            // create storages
            char[] plaintextLettersOnly = new char[ciphertext.Length];
            int plaintextOffset = 0;

            int keyOffset = 0;

            int i;

            // construct the initial part of the plaintext
            for (i = 0; i < ciphertext.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(ciphertext[i]);

                if (idx != -1)
                {
                    result[i] = this.Charset[(idx - this.Charset.IndexOf(key[plaintextOffset])).Mod(this.Charset.Length)].ToSameCaseAs(ciphertext[i]);
                    plaintextLettersOnly[plaintextOffset] = Char.ToUpper(result[i]);
                    plaintextOffset++;

                    if (plaintextOffset == key.Length)
                        break;
                }
                else
                    result[i] = ciphertext[i];
            }

            // decipher the remaining message
            for (i++; i < ciphertext.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(ciphertext[i]);

                if (idx != -1)
                {
                    result[i] = this.Charset[(idx - this.Charset.IndexOf(plaintextLettersOnly[plaintextOffset - key.Length])).Mod(this.Charset.Length)].ToSameCaseAs(ciphertext[i]);
                    plaintextLettersOnly[plaintextOffset] = Char.ToUpper(result[i]);
                    plaintextOffset++;

                    if (keyOffset == key.Length)
                        break;
                }
                else
                    result[i] = ciphertext[i];
            }

            return new String(result);
        }
    }
}
