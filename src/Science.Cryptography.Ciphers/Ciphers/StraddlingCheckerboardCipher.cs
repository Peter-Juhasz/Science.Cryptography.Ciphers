using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

[Export("StraddlingCheckerboard", typeof(IKeyedCipher<StraddlingCheckerboard>))]
public class StraddlingCheckerboardCipher : IKeyedCipher<StraddlingCheckerboard>
{
	public int GetMaxOutputCharactersPerInputCharacter(StraddlingCheckerboard key) => 2;

	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, StraddlingCheckerboard key, out int written)
	{
		if (ciphertext.Length < plaintext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(ciphertext));
		}

		var writer = new SpanWriter<char>(ciphertext);

		foreach (var ch in plaintext)
		{
			if (key.TryFindOffsets(ch, out (int row, int column) position))
			{
				if (position.row != StraddlingCheckerboard.EmptyIndex)
				{
					position.row.TryFormat(writer.GetSpan(1), out _);
				}
				position.column.TryFormat(writer.GetSpan(1), out _);
			}
			else
			{
				writer.Write(ch);
			}
		}

		written = writer.Written;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, StraddlingCheckerboard key, out int written)
	{
		var writer = new SpanWriter<char>(plaintext);

		var namedIndex = key.GetNamedIndexes();

		var lastIndex = ciphertext.Length;
		var beforeLastIndex = ciphertext.Length - 1;
		for (int i = 0; i < lastIndex; i++)
		{
			if (!Int32.TryParse(ciphertext.Slice(i, 1), out var firstLabelIndex))
			{
				writer.Write(ciphertext[i]);
				continue;
			}

			if (!namedIndex.Contains(firstLabelIndex))
			{
				var result = key[StraddlingCheckerboard.EmptyIndex, firstLabelIndex];
				if (result != StraddlingCheckerboard.EmptyValue)
				{
					writer.Write(result);
					continue;
				}
				else
				{
					writer.Write(ciphertext[i]);
					continue;
				}
			}
			else if (i < beforeLastIndex)
			{
				if (!Int32.TryParse(ciphertext.Slice(i + 1, 1), out var secondLabelIndex))
				{
					writer.Write(ciphertext[i]);
					continue;
				}

				var result = key[firstLabelIndex, secondLabelIndex];
				if (result != StraddlingCheckerboard.EmptyValue)
				{
					writer.Write(result);
					i++;
					continue;
				}
				else
				{
					writer.Write(ciphertext[i]);
					continue;
				}
			}
		}

		written = writer.Written;
	}
}
