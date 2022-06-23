using System;

namespace Science.Cryptography.Ciphers;

public static class IntArrayKey
{
	public static bool TryFromCharIndexesOfAlphabet(ReadOnlySpan<char> keyword, Alphabet alphabet, Span<int> result, int firstIndex = 1)
	{
		if (result.Length < keyword.Length)
		{
			return false;
		}

		for (int i = 0; i < keyword.Length; i++)
		{
			int index = alphabet.IndexOfIgnoreCase(keyword[i]);
			if (index == -1)
			{
				return false;
			}

			result[i] = firstIndex + index;
		}

		return true;
	}

	public static bool TryFromCharIndexesOfAlphabetSequential(ReadOnlySpan<char> keyword, Alphabet alphabet, Span<int> result, int firstIndex = 1)
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

		int num = 0;

		int min = Min(indexes, -1);
		do
		{
			for (int i = 0; i < indexes.Length; i++)
			{
				int value = indexes[i];
				if (value == min)
				{
					result[i] = num + firstIndex;
				}
			}
			min = Min(indexes, min);
			num++;
		} while (min != Int32.MaxValue);

		return true;
	}

	public static bool TryFromCharIndexesOfAlphabetSorted(ReadOnlySpan<char> keyword, Alphabet alphabet, Span<int> result, int firstIndex = 1)
	{
		if (TryFromCharIndexesOfAlphabet(keyword, alphabet, result, firstIndex))
		{
			result.Sort();
			return true;
		}

		return false;
	}

	private static int Min(ReadOnlySpan<int> span, int threshold)
	{
		int min = Int32.MaxValue;
		foreach (int value in span)
		{
			if (value > threshold && value < min)
			{
				min = value;
			}
		}
		return min;
	}

	public static int[] FromCharIndexesOfAlphabet(ReadOnlySpan<char> keyword, Alphabet alphabet, int firstIndex = 1)
	{
		var result = new int[keyword.Length];
		if (!TryFromCharIndexesOfAlphabet(keyword, alphabet, result, firstIndex))
		{
			throw new InvalidOperationException();
		}
		return result;
	}

	public static int[] FromCharIndexesOfAlphabetSorted(ReadOnlySpan<char> keyword, Alphabet alphabet, int firstIndex = 1)
	{
		var result = new int[keyword.Length];
		if (!TryFromCharIndexesOfAlphabetSorted(keyword, alphabet, result, firstIndex))
		{
			throw new InvalidOperationException();
		}
		return result;
	}

	public static int[] FromCharIndexesOfAlphabetSequential(ReadOnlySpan<char> keyword, Alphabet alphabet, int firstIndex = 1)
	{
		var result = new int[keyword.Length];
		if (!TryFromCharIndexesOfAlphabetSequential(keyword, alphabet, result, firstIndex))
		{
			throw new InvalidOperationException();
		}
		return result;
	}
}
