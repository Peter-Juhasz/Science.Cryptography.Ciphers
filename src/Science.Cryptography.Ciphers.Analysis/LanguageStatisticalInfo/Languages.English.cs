using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis
{
    public static partial class Languages
    {
        public static readonly ILanguageStatisticalInfo English = new LanguageStatisticalInfo(
            relativeFrequenciesOfLetters: new Dictionary<char, double>()
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
            },
            relativeFrequenciesOfFirstLettersOfWords: new Dictionary<char, double>()
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
            }
        );
    }
}
