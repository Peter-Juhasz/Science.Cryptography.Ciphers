using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
/// </summary>
public sealed class RelativeLetterFrequenciesSpeculativePlaintextScorer : ISpeculativePlaintextScorer, IPartitionedSpeculativePlaintextScorer
{
	public RelativeLetterFrequenciesSpeculativePlaintextScorer(RelativeCharacterFrequencies relativeFrequencies)
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
		return FrequencyAnalysis.Compare(_relativeFrequencies, FrequencyAnalysis.AnalyzeLetters(speculativePlaintext));
	}

	public ISpeculativePlaintextScorer GetForPartition() => new LetterPartitionedScorer(_relativeFrequencies.Frequencies);


	private sealed class LetterPartitionedScorer : ISpeculativePlaintextScorer
	{
		public LetterPartitionedScorer(IReadOnlyDictionary<char, double> reference)
		{
			_reference = reference;
		}

		private readonly IReadOnlyDictionary<char, double> _reference;
		private readonly Dictionary<char, int> _buffer = new(capacity: 26);

		public double Score(ReadOnlySpan<char> speculativePlaintext)
		{
			FrequencyAnalysis.AnalyzeLetters(speculativePlaintext, _buffer);
			var score = FrequencyAnalysis.CompareCore(_reference, _buffer);
			_buffer.Clear();
			return score;
		}
	}
}
