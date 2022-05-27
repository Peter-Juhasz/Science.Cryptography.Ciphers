using System;
using System.Runtime.InteropServices;

namespace Science.Cryptography.Ciphers;

internal static class ArrayHelper
{
	public static void FillSlow(char[,] buffer, ReadOnlySpan<char> source, int size)
	{
		for (int i = 0; i < source.Length; i++)
		{
			buffer[i / size, i % size] = source[i].ToUpper();
		}
	}

	public static void FillTransposedSlow(char[,] buffer, ReadOnlySpan<char> source, int size)
	{
		for (int i = 0; i < source.Length; i++)
		{
			buffer[i % size, i / size] = source[i].ToUpper();
		}
	}

	public static void FillFast(char[,] buffer, ReadOnlySpan<char> source, int size) => source.CopyTo(buffer.AsSpan(size));

	public static void FillWithKeywordAndAlphabet(Span<char> buffer, ReadOnlySpan<char> keyword, ReadOnlySpan<char> alphabet, bool throwOnDuplicates = false)
	{
		int written = 0;

		for (int i = 0; i < keyword.Length; i++)
		{
			var upper = keyword[i].ToUpper();
			if (buffer.IndexOf(upper) == -1)
			{
				buffer[written++] = upper;
			}
		}

		if (throwOnDuplicates && written < keyword.Length)
		{
			throw new ArgumentOutOfRangeException(nameof(keyword), "Keyword contains duplicate characters.");
		}

		for (int i = 0; i < alphabet.Length; i++)
		{
			var upper = alphabet[i].ToUpper();
			if (buffer.IndexOf(upper) == -1)
			{
				buffer[written++] = upper;
			}
		}
	}

	public static bool TryFindOffsets(char[,] buffer, char ch, out (int row, int column) positions, int rows, int columns)
	{
		char upper = ch.ToUpper();

		int index = buffer.AsSpan(rows, columns).IndexOf(upper); 
		if (index > -1)
		{
			positions = (row: index / columns, column: index % columns);
			return true;
		}

		positions = default;
		return false;
	}

	public static bool TryFindOffsets(char[,] buffer, char ch, out (int row, int column) positions, int size) =>
		TryFindOffsets(buffer, ch, out positions, size, size);

	public static bool TryFindOffsetsSlow(char[,] square, char ch, out (int row, int column) positions, int rows, int columns)
	{
		char upper = ch.ToUpper();

		for (int row = 0; row < rows; row++)
		for (int column = 0; column < columns; column++)
		{
			if (square[row, column] == upper)
			{
				positions = (row, column);
				return true;
			}
		}

		positions = default;
		return false;
	}

	public static Span<T> AsSpan<T>(this T[,] array, int rows, int columns) => MemoryMarshal.CreateSpan(ref array[0, 0], columns * rows);

	public static Span<T> AsSpan<T>(this T[,] array, int size) => array.AsSpan(size, size);
}
