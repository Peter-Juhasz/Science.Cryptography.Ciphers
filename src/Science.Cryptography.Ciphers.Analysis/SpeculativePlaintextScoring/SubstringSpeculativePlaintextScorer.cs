using System;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
/// </summary>
public sealed class SubstringSpeculativePlaintextScorer : ISpeculativePlaintextScorer
{
	public SubstringSpeculativePlaintextScorer(string substring, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
	{
		Substring = substring;
		Comparison = comparison;
	}

	public string Substring { get; }
	public StringComparison Comparison { get; }


	/// <summary>
	/// Return 1 when the substring is found in the candidate, 0 if not.
	/// </summary>
	/// <param name="speculativePlaintext"></param>
	/// <returns></returns>
	public double Score(ReadOnlySpan<char> speculativePlaintext) => speculativePlaintext.Contains(Substring, Comparison) ? 1D : 0D;
}
