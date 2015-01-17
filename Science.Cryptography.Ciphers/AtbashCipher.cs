using System;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Atbash cipher.
    /// </summary>
    public class AtbashCipher : ICipher, ISupportsCustomCharset
    {
        public AtbashCipher()
        {
            this.Charset = Charsets.English;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }

        protected string Crypt(string text)
        {
            char[] ciphertext = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(text[i]);

                if (idx != -1)
                    ciphertext[i] = (this.Charset[this.Charset.Length - idx - 1]).ToSameCaseAs(text[i]);
                else
                    ciphertext[i] = text[i];
            }

            return new String(ciphertext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public string Encrypt(string plaintext)
        {
            return this.Crypt(plaintext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public string Decrypt(string ciphertext)
        {
            return this.Crypt(ciphertext);
        }
    }
}
