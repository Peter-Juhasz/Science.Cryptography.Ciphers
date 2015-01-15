﻿using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Streaming
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
        protected IEnumerable<char> Crypt(IEnumerable<char> text, int key)
        {
            foreach (char c in text)
            {
                int idx = this.Charset.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase);

                yield return idx != -1
                    ? (this.Charset[Mod(idx + key, this.Charset.Length)]).ToSameCaseAs(c)
                    : c
                ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public IEnumerable<char> Encrypt(IEnumerable<char> plaintext, int key)
        {
            return this.Crypt(plaintext, key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public IEnumerable<char> Decrypt(IEnumerable<char> ciphertext, int key)
        {
            return this.Crypt(ciphertext, -key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        internal int Mod(int a, int b)
        {
            return a >= 0 ? a % b : (b + a) % b;
        }
    }
}