﻿using System;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Running Key cipher.
    /// </summary>
    public class RunningKeyCipher : IKeyedCipher<string>, ISupportsCustomCharset
    {
        public RunningKeyCipher()
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

            key = key.ToUpper();

            for (int i = 0; i < plaintext.Length; i++)
            {
                int idx = this.Charset.IndexOf(plaintext[i].ToString(), StringComparison.OrdinalIgnoreCase);

                if (idx != -1)
                    result[i] = (this.Charset[(idx + this.Charset.IndexOf(key[charCounter++])) % this.Charset.Length]).ToSameCaseAs(plaintext[i]);
                else
                    result[i] = plaintext[i];
            }

            return new String(result);
        }

        public string Decrypt(string ciphertext, string key)
        {
            char[] result = new char[ciphertext.Length];
            int charCounter = 0;

            key = key.ToUpper();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                int idx = this.Charset.IndexOf(ciphertext[i].ToString(), StringComparison.OrdinalIgnoreCase);

                if (idx != -1)
                    result[i] = (this.Charset[Mod(idx - this.Charset.IndexOf(key[charCounter++]), this.Charset.Length)]).ToSameCaseAs(ciphertext[i]);
                else
                    result[i] = ciphertext[i];
            }

            return new String(result);
        }

        internal static int Mod(int a, int b)
        {
            return a >= 0 ? a % b : (b + a) % b;
        }
    }
}