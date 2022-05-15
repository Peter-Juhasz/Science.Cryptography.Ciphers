using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

internal static class StringExtensions
{
	public static IEnumerable<int> AllIndexesOf(this string str, string subject, int startIndex = 0, StringComparison comparison = StringComparison.Ordinal)
	{
		int idx = str.IndexOf(subject, startIndex, comparison);

		while (idx != -1)
		{
			yield return idx;
			idx = str.IndexOf(subject, idx + 1, comparison);
		}
	}
}
