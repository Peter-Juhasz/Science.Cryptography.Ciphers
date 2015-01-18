using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Contains n-gram analysis tools.
    /// </summary>
    public static class NGramAnalysis
    {
        /// <summary>
        /// Converts a given <paramref name="text"/> to <paramref name="n"/>-length sequences (including overlapping ones).
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <param name="n">The length of the sequences.</param>
        /// <returns>All <paramref name="n"/>-grams.</returns>
        /// <example>ABCD to 2-grams: AB, BC, CD</example>
        public static IEnumerable<string> ReadNGrams(this string text, int n)
        {
            if (n <= 0)
                throw new ArgumentOutOfRangeException("n");

            StringBuilder window = new StringBuilder(n);

            for (int i = 0; i <= text.Length - n; i++)
            {
                window.Clear();

                for (int j = 0; j < n; j++)
                    window.Append(text[i + j]);

                yield return window.ToString();
            }
        }

        /// <summary>
        /// Measures <paramref name="n"/>-gram frequencies in a given text.
        /// </summary>
        /// <param name="text">Text to analyze.</param>
        /// <param name="n">The length of n-gram sequencies.</param>
        /// <returns>Frequencies of <paramref name="n"/>-grams in the text.</returns>
        public static IReadOnlyDictionary<string, int> Analyze(string text, int n)
        {
            return text.ReadNGrams(n)
                .GroupBy(s => s)
                .ToDictionary(g => g.Key, g => g.Count())
            ;
        }
    }
}
