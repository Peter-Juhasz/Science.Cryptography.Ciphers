using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis
{
    public interface ILanguageReferenceData
    {
        IReadOnlyDictionary<char, double> RelativeFrequenciesOfLetters { get; }

        IReadOnlyDictionary<char, double> RelativeFrequenciesOfFirstLettersOfWords { get; }
    }
}
