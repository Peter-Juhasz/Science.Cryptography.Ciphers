using System;
using System.Composition;
using System.Runtime.CompilerServices;

namespace Science.Cryptography.Ciphers;

[Export("ColumnarTransposition", typeof(IKeyedCipher<>))]
public class ColumnarTranspositionCipher : IKeyedCipher<int[]>
{
	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, int[] key, out int written)
	{
		var writer = new SpanWriter<char>(ciphertext);

		var keyLength = key.Length;
		var fullRows = plaintext.Length / keyLength;

		Span<int> sortedKeyIndices = stackalloc int[keyLength];
		ComputeSortedKeyIndicesInputStartsFromOne(key, sortedKeyIndices);

		foreach (var keyIndex in sortedKeyIndices)
		{
			for (int i = 0; i < fullRows; i++)
			{
				writer.Write(plaintext[keyLength * i + keyIndex]);
			}

			var lastPossible = keyLength * fullRows + keyIndex;
			if (lastPossible < plaintext.Length)
			{
				writer.Write(plaintext[lastPossible]);
			}
		}

		written = writer.Written;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, int[] key, out int written)
	{
		var keyLength = key.Length;
		var (fullRows, overflowColumns) = Math.DivRem(ciphertext.Length, keyLength);

		Span<int> columnHeights = stackalloc int[keyLength];
		columnHeights.Fill(fullRows);
		for (int o = 0; o < overflowColumns; o++)
		{
			columnHeights[key[o] - 1]++;
		}

		Span<int> columnStartPositions = stackalloc int[keyLength];
		for (int i = 0; i < keyLength; i++)
		{
			int sum = 0;
			int currentKey = key[i] - 1;
			for (int j = 0; j < currentKey; j++)
			{
				sum += columnHeights[j];
			}
			columnStartPositions[i] = sum;
		}

		for (int i = 0; i < ciphertext.Length; i++)
		{
			var (rowIndex, columnIndex) = Math.DivRem(i, keyLength);
			var currentKey = key[columnIndex] - 1;
			var startPosition = columnStartPositions[columnIndex];

			plaintext[i] = ciphertext[startPosition + rowIndex];
		}

		written = ciphertext.Length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void ComputeSortedKeyIndicesInputStartsFromOne(ReadOnlySpan<int> key, Span<int> sortedKeyIndices)
	{
		for (int i = 0; i < key.Length; i++)
		{
			sortedKeyIndices[key[i] - 1] = i;
		}
	}
}
