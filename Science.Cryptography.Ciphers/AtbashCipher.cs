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
            char[] result = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(text[i]);

                result[i] = idx != -1
                    ? this.Charset[this.Charset.Length - idx - 1].ToSameCaseAs(text[i])
                    : text[i]
                ;
            }

            return new String(result);
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
