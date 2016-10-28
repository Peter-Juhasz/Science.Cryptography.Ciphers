using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Contains reference data for frequency analysis.
    /// </summary>
    public static class RelativeFrequenciesOfLettersByLanguages
    {
        /// <summary>
        /// Relative frequencies of letters in the English language.
        /// </summary>
        public static readonly IReadOnlyDictionary<char, double> English = new Dictionary<char, double>()
        {
            { 'a', 0.08167 },
            { 'b', 0.01492 },
            { 'c', 0.02782 },
            { 'd', 0.04253 },
            { 'e', 0.12702 },
            { 'f', 0.02228 },
            { 'g', 0.02015 },
            { 'h', 0.06094 },
            { 'i', 0.06966 },
            { 'j', 0.00153 },
            { 'k', 0.00772 },
            { 'l', 0.04025 },
            { 'm', 0.02406 },
            { 'n', 0.06749 },
            { 'o', 0.07507 },
            { 'p', 0.01929 },
            { 'q', 0.00095 },
            { 'r', 0.05987 },
            { 's', 0.06327 },
            { 't', 0.09056 },
            { 'u', 0.02758 },
            { 'v', 0.00978 },
            { 'w', 0.02360 },
            { 'x', 0.00150 },
            { 'y', 0.01975 },
            { 'z', 0.00074 },
        };
    }
}
