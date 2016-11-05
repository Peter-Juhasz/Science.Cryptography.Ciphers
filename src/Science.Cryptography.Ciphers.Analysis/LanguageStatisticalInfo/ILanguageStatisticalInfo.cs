namespace Science.Cryptography.Ciphers.Analysis
{
    public interface ILanguageStatisticalInfo
    {
        RelativeCharacterFrequencies RelativeFrequenciesOfLetters { get; }

        RelativeCharacterFrequencies RelativeFrequenciesOfFirstLettersOfWords { get; }
    }
}
