using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis
{
    internal sealed class LanguageStatisticalInfo : ILanguageStatisticalInfo
    {
        public LanguageStatisticalInfo(
            IReadOnlyDictionary<char, double> relativeFrequenciesOfLetters,
            IReadOnlyDictionary<char, double> relativeFrequenciesOfFirstLettersOfWords
        )
        {
            this.RelativeFrequenciesOfLetters = relativeFrequenciesOfLetters;
            this.RelativeFrequenciesOfFirstLettersOfWords = relativeFrequenciesOfFirstLettersOfWords;
        }

        public IReadOnlyDictionary<char, double> RelativeFrequenciesOfLetters { get; private set; }

        public IReadOnlyDictionary<char, double> RelativeFrequenciesOfFirstLettersOfWords { get; private set; }
    }
}
