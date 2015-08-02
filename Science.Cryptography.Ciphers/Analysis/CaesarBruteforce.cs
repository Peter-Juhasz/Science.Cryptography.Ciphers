using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Analysis method for breaking Shift ciphers.
    /// </summary>
    public static class CaesarBruteforce
    {
        /// <summary>
        /// Computes every possible transformations of the input text.
        /// </summary>
        /// <param name="text">The input text to transform.</param>
        /// <returns></returns>
        public static IReadOnlyDictionary<int, string> Analyze(string text)
        {
            return Analyze(text, Charsets.English);
        }
        /// <summary>
        /// Computes every possible transformations of the input text.
        /// </summary>
        /// <param name="text">The input text to transform.</param>
        /// <param name="charset">Charset of the input text.</param>
        /// <returns></returns>
        public static IReadOnlyDictionary<int, string> Analyze(string text, string charset)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (charset == null)
                throw new ArgumentNullException(nameof(charset));


            var cipher = new ShiftCipher() { Charset = charset };

            return Enumerable.Range(0, charset.Length)
                .ToDictionary(k => k, k => cipher.Encrypt(text, k))
            ;
        }
    }
}
