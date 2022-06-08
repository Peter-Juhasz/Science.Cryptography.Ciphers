using System;
using System.Text.RegularExpressions;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
/// </summary>
public sealed class RegexSpeculativePlaintextScorer : ISpeculativePlaintextScorer
{
	public RegexSpeculativePlaintextScorer(Regex regex)
	{
		RegularExpression = regex;
	}

	public Regex RegularExpression { get; }


	/// <summary>
	/// Return 1 when the substring is found in the candidate, 0 if not.
	/// </summary>
	/// <param name="speculativePlaintext"></param>
	/// <returns></returns>
	public double Score(ReadOnlySpan<char> speculativePlaintext) => RegularExpression.IsMatch(new string(speculativePlaintext)) switch // TODO: upgrade to .net 7 api
	{
		true => 1D,
		false => 0D
	};
}
