using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis;

public sealed class TwoGramAsciiLettersRelativeFrequenciesSpeculativePlaintextScorer : ISpeculativePlaintextScorer, IPartitionedSpeculativePlaintextScorer
{
	public TwoGramAsciiLettersRelativeFrequenciesSpeculativePlaintextScorer(RelativeStringFrequencies relativeFrequencies)
	{
		if (relativeFrequencies.Any(f => f.Key.Length != 2))
		{
			throw new ArgumentOutOfRangeException(nameof(relativeFrequencies));
		}

		_relativeFrequencies = relativeFrequencies;
		_twoGramReference = _relativeFrequencies.ToDictionary(k => NGramAnalysis.GetTwoGramKey(k.Key), k => k.Value);
		_mainPartition = GetForPartition();
	}

	private readonly RelativeStringFrequencies _relativeFrequencies;
	private readonly IReadOnlyDictionary<int, double> _twoGramReference;
	private readonly ISpeculativePlaintextScorer _mainPartition;

	public ISpeculativePlaintextScorer GetForPartition() => new TwoGramAsciiLetterPartitionedScorer(_twoGramReference);

	public double Score(ReadOnlySpan<char> speculativePlaintext) => _mainPartition.Score(speculativePlaintext);

	private sealed class TwoGramAsciiLetterPartitionedScorer : ISpeculativePlaintextScorer
	{
		public TwoGramAsciiLetterPartitionedScorer(IReadOnlyDictionary<int, double> reference)
		{
			_reference = reference;
		}

		private readonly IReadOnlyDictionary<int, double> _reference;
		private readonly Dictionary<int, int> _buffer = new(capacity: 26 * 26);

		public double Score(ReadOnlySpan<char> speculativePlaintext)
		{
			NGramAnalysis.AnalyzeAsciiLetterTwoGrams(speculativePlaintext, _buffer);
			var score = FrequencyAnalysis.CompareCore(_reference, _buffer);
			_buffer.Clear();
			return score;
		}
	}
}
