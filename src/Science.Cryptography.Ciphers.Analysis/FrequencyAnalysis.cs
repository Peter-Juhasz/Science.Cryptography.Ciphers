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

        
        /// <summary>
        /// Compares two set of relative frequencies.
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static double Compare(IReadOnlyDictionary<char, double> reference, IReadOnlyDictionary<char, double> subject)
        {
            return 1 - (
                from r in reference
                select Math.Abs(r.Value - (subject.ContainsKey(r.Key) ? subject[r.Key] : 0))
            ).Sum();
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
