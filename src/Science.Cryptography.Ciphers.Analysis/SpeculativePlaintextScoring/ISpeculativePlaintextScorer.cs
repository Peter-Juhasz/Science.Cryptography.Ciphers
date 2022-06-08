using System;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Classifies potential plaintext candidates on how likely are they meaningful results.
/// </summary>
public interface ISpeculativePlaintextScorer
{
	/// <summary>
	/// Scores the candidate between 0 and 1. The higher the score is the more likely the candidate is meaningful text.
	/// </summary>
	/// <param name="speculativePlaintext"></param>
	/// <returns></returns>
	double Score(ReadOnlySpan<char> speculativePlaintext);
}
