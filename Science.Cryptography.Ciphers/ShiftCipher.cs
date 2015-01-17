using System;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Shift cipher.
    /// </summary>
    public class ShiftCipher : IKeyedCipher<int>, ISupportsCustomCharset
    {
        public ShiftCipher()
        {
            this.Charset = Charsets.English;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string Crypt(string text, int key)
        {
            char[] result = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(text[i]);

                if (idx != -1)
                    result[i] = (this.Charset[(idx + key).Mod(this.Charset.Length)]).ToSameCaseAs(text[i]);
                else
                    result[i] = text[i];
            }

            return new String(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public string Encrypt(string plaintext, int key)
        {
            return this.Crypt(plaintext, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public string Decrypt(string ciphertext, int key)
        {
            return this.Crypt(ciphertext, -key);
        }
    }

    /// <summary>
    /// Contains well-known cipher keys for <see cref="ShiftCipher"/>.
    /// </summary>
    public static class WellKnownShiftCipherKeys
    {
        /// <summary>
        /// Julius Caesar's key for the shift cipher.
        /// </summary>
        public const int Caesar = 3;

        /// <summary>
        /// Key for the ROT-13 chiper.
        /// </summary>
        public const int Rot13 = 13;
    }
}
