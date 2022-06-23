using System;

namespace Science.Cryptography.Ciphers;

public static class CharArrayKey
{
	public static bool TrySortByAlphabet(ReadOnlySpan<char> keyword, Alphabet alphabet, Span<char> result)
	{
		if (result.Length < keyword.Length)
		{
			return false;
		}

		Span<int> indexes = stackalloc int[keyword.Length];
		for (int i = 0; i < keyword.Length; i++)
		{
			int index = alphabet.IndexOfIgnoreCase(keyword[i]);
			if (index == -1)
			{
				return false;
			}
			indexes[i] = index;
		}

		indexes.Sort();

		for (int i = 0; i < indexes.Length; i++)
		{
			result[i] = alphabet[indexes[i]];
		}

		return true;
	}

	public static char[] SortByAlphabet(ReadOnlySpan<char> keyword, Alphabet alphabet)
	{
		var result = new char[keyword.Length];
		if (!TrySortByAlphabet(keyword, alphabet, result))
		{
			throw new InvalidOperationException();
		}
		return result;
	}
}
