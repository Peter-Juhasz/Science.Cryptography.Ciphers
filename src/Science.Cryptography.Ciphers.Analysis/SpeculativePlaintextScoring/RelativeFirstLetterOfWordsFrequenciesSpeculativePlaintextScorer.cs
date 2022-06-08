using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
/// </summary>
public sealed class RelativeFirstLetterOfWordsFrequenciesSpeculativePlaintextScorer : ISpeculativePlaintextScorer
{
	public RelativeFirstLetterOfWordsFrequenciesSpeculativePlaintextScorer(RelativeCharacterFrequencies relativeFrequencies)
	{
		_relativeFrequencies = relativeFrequencies;
	}


	private readonly RelativeCharacterFrequencies _relativeFrequencies;


	/// <summary>
	/// Return 1 when the substring is found in the candidate, 0 if not.
	/// </summary>
	/// <param name="speculativePlaintext"></param>
	/// <returns></returns>
	public double Score(ReadOnlySpan<char> speculativePlaintext)
	{
		return FrequencyAnalysis.Compare(_relativeFrequencies, Analyze(speculativePlaintext));
	}


	internal static AbsoluteCharacterFrequencies Analyze(ReadOnlySpan<char> text)
	{
		Dictionary<char, int> frequencies = new Dictionary<char, int>(IgnoreCaseCharComparer.Instance);

		if (text.Length > 0 && Char.IsLetter(text[0]))
		{
			frequencies[text[0]] = 1;
		}

		for (int i = 1; i < text.Length; i++)
		{
			char current = text[i];

			if (Char.IsLetter(current) && !Char.IsLetter(text[i - 1]))
			{
				if (frequencies.ContainsKey(current))
					frequencies[current]++;
				else
					frequencies[current] = 1;
			}
		}

		return new AbsoluteCharacterFrequencies(frequencies);
	}
}
