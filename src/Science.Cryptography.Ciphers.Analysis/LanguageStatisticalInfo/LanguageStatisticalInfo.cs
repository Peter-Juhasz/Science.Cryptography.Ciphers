namespace Science.Cryptography.Ciphers.Analysis
{
    internal sealed class LanguageStatisticalInfo : ILanguageStatisticalInfo
    {
        public LanguageStatisticalInfo(
            RelativeCharacterFrequencies relativeFrequenciesOfLetters,
            RelativeCharacterFrequencies relativeFrequenciesOfFirstLettersOfWords
        )
        {
            this.RelativeFrequenciesOfLetters = relativeFrequenciesOfLetters;
            this.RelativeFrequenciesOfFirstLettersOfWords = relativeFrequenciesOfFirstLettersOfWords;
        }

        public RelativeCharacterFrequencies RelativeFrequenciesOfLetters { get; private set; }

        public RelativeCharacterFrequencies RelativeFrequenciesOfFirstLettersOfWords { get; private set; }
    }
}
