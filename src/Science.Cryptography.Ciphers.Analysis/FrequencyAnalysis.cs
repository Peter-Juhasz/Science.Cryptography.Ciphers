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
                throw new ArgumentNullException(nameof(text));

            return text
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count())
            ;
        }

        
        public static double Compare(IReadOnlyDictionary<char, double> reference, IReadOnlyDictionary<char, double> right)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts absolute frequencies to relative frequencies.
        /// </summary>
        /// <param name="frequencies"></param>
        /// <returns></returns>
        public static IReadOnlyDictionary<char, double> AsRelativeFrequencies(this IReadOnlyDictionary<char, int> frequencies)
        {
            return frequencies.ToDictionary(
                kv => kv.Key,
                kv => (double)kv.Value / frequencies.Sum(f => f.Value)
            );
        }
    }
}
