using System;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
/// </summary>
public sealed class RelativeNGramFrequenciesSpeculativePlaintextScorer : ISpeculativePlaintextScorer
{
	public RelativeNGramFrequenciesSpeculativePlaintextScorer(RelativeStringFrequencies relativeFrequencies, int length)
	{
		if (relativeFrequencies.Any(f => f.Key.Length != length))
		{
			throw new ArgumentOutOfRangeException(nameof(relativeFrequencies));
		}

		_relativeFrequencies = relativeFrequencies;
		_length = length;
	}

	private readonly RelativeStringFrequencies _relativeFrequencies;
	private readonly int _length;


	/// <summary>
	/// Return 1 when the substring is found in the candidate, 0 if not.
	/// </summary>
	/// <param name="speculativePlaintext"></param>
	/// <returns></returns>
	public double Score(ReadOnlySpan<char> speculativePlaintext)
	{
		var frequencies = NGramAnalysis.AnalyzeLetters(new string(speculativePlaintext), _length);
		return FrequencyAnalysis.Compare(_relativeFrequencies, frequencies);
	}
}
