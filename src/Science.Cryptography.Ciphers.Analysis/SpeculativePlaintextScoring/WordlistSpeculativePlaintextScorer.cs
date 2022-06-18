using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
/// </summary>
public sealed class WordlistSpeculativePlaintextScorer : ISpeculativePlaintextScorer
{
	public WordlistSpeculativePlaintextScorer(IReadOnlyCollection<string> wordlist, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
	{
		_orderedWordlist = wordlist.OrderByDescending(w => w.Length).ToList();
		_comparison = comparison;
	}

	private readonly IReadOnlyList<string> _orderedWordlist;
	private readonly StringComparison _comparison;


	/// <summary>
	/// Return 1 when the substring is found in the candidate, 0 if not.
	/// </summary>
	/// <param name="speculativePlaintext"></param>
	/// <returns></returns>
	public double Score(ReadOnlySpan<char> speculativePlaintext)
	{
		Span<ulong> bitsBuffer = stackalloc ulong[ValueBitArray.GetNumberOfBucketsForBits(speculativePlaintext.Length)];
		var bitArray = new ValueBitArray(bitsBuffer);

		foreach (var word in _orderedWordlist.AsValueEnumerable())
		{
			var index = speculativePlaintext.IndexOf(word, _comparison);
			while (index != -1)
			{
				bitArray.SetRangeToOne(index, word.Length);

				var nextStartIndex = index + word.Length;
				if (nextStartIndex + word.Length > speculativePlaintext.Length)
				{
					break;
				}

				int relativeIndex = speculativePlaintext[nextStartIndex..].IndexOf(word, _comparison);
				if (relativeIndex == -1)
				{
					break;
				}

				index = relativeIndex + nextStartIndex;
			}
		}

		return bitArray.GetCardinality() / (double)speculativePlaintext.Length;
	}
}
