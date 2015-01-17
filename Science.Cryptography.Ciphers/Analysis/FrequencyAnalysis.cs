using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Measures character frequencies.
    /// </summary>
    public static class FrequencyAnalysis
    {
        /// <summary>
        /// Measures character frequencies in a given text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<char, int> Analyze(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            return text.AsEnumerable()
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count())
            ;
        }
    }
}
