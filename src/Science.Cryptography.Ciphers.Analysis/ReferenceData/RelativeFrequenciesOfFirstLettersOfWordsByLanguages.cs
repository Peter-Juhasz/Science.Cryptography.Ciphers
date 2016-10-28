using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Contains reference data for frequency analysis.
    /// </summary>
    public static class RelativeFrequenciesOfFirstLettersOfWordsByLanguages
    {
        /// <summary>
        /// Relative frequencies of first letters of words in the English language.
        /// </summary>
        public static readonly IReadOnlyDictionary<char, double> English = new Dictionary<char, double>()
        {
            { 'a', 0.11602 },
            { 'b', 0.04702 },
            { 'c', 0.03511 },
            { 'd', 0.02670 },
            { 'e', 0.02007 },
            { 'f', 0.03779 },
            { 'g', 0.01950 },
            { 'h', 0.07232 },
            { 'i', 0.06286 },
            { 'j', 0.00597 },
            { 'k', 0.00590 },
            { 'l', 0.02705 },
            { 'm', 0.04383 },
            { 'n', 0.02365 },
            { 'o', 0.06264 },
            { 'p', 0.02545 },
            { 'q', 0.00173 },
            { 'r', 0.01653 },
            { 's', 0.07755 },
            { 't', 0.16671 },
            { 'u', 0.01487 },
            { 'v', 0.00649 },
            { 'w', 0.06753 },
            { 'x', 0.00017 },
            { 'y', 0.01620 },
            { 'z', 0.00034 },
        };
    }
}
