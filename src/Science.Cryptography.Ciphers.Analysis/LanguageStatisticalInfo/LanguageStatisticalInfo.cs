using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

public sealed record class LanguageStatisticalInfo(
	string Name,
	string TwoLetterISOCode,
	Alphabet Alphabet,
	RelativeCharacterFrequencies RelativeFrequenciesOfLetters,
	RelativeCharacterFrequencies RelativeFrequenciesOfFirstLettersOfWords,
	IReadOnlyDictionary<int, RelativeStringFrequencies> RelativeNGramFrequencies
)
{
	public RelativeStringFrequencies GetNGramFrequencies(int length) => RelativeNGramFrequencies[length];

	public override string ToString() => Name;
}
