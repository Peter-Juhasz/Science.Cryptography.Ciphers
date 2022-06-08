using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
/// </summary>
public sealed class AnySubstringSpeculativePlaintextScorer : ISpeculativePlaintextScorer
{
	public AnySubstringSpeculativePlaintextScorer(IReadOnlySet<string> substrings, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
	{
		Substrings = substrings;
		Comparison = comparison;
		_inner = Substrings.ToArray();
	}

	private readonly string[] _inner;
	public IReadOnlySet<string> Substrings { get; }
	public StringComparison Comparison { get; }


	/// <summary>
	/// Return 1 when the substring is found in the candidate, 0 if not.
	/// </summary>
	/// <param name="speculativePlaintext"></param>
	/// <returns></returns>
	public double Score(ReadOnlySpan<char> speculativePlaintext)
	{
		foreach (var substring in _inner.AsSpan())
		{
			if (speculativePlaintext.Contains(substring, Comparison))
			{
				return 1D;
			}
		}

		return 0D;
	}
}
