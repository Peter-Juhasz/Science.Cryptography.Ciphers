using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Science.Cryptography.Ciphers.Analysis;

public static partial class NGramAnalysis
{
	internal static void AnalyzeAsciiLetterTwoGrams(ReadOnlySpan<char> text, Dictionary<int, int> result)
	{
		var lastIndex = text.Length - 2;
		for (int i = 0; i <= lastIndex; i++)
		{
			var c1 = text[i].ToUpperInvariant();
			if (!c1.IsUpperAsciiLetter())
			{
				continue;
			}

			var c2 = text[i + 1].ToUpperInvariant();
			if (!c2.IsUpperAsciiLetter())
			{
				i++;
				continue;
			}

			var key = GetTwoGramKey(c1, c2);
			ref int frequency = ref CollectionsMarshal.GetValueRefOrAddDefault(result, key, out _);
			frequency++;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static int GetTwoGramKey(ReadOnlySpan<char> segment)
	{
		if (segment.Length != 2)
		{
			throw new ArgumentOutOfRangeException(nameof(segment));
		}

		return GetTwoGramKey(segment[0].ToUpperInvariant(), segment[1].ToUpperInvariant());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static int GetTwoGramKey(char c1, char c2) => (c1 << 16) + c2;
}