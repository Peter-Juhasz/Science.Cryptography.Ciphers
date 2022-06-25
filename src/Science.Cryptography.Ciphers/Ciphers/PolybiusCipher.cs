using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

[Export("Polybius", typeof(IKeyedCipher<PolybiusSquare>))]
public class PolybiusCipher : IKeyedCipher<PolybiusSquare>
{
	public int GetMaxOutputCharactersPerInputCharacter(PolybiusSquare key) => 2;

	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, PolybiusSquare key, out int written)
	{
		if (ciphertext.Length < plaintext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(ciphertext));
		}

		var writer = new SpanWriter<char>(ciphertext);

		for (int i = 0; i < plaintext.Length; i++)
		{
			var ch = plaintext[i];
			if (key.TryFindOffsets(ch, out (int row, int column) position))
			{
				(position.row + 1).TryFormat(writer.GetSpan(1), out _);
				(position.column + 1).TryFormat(writer.GetSpan(1), out _);
			}
			else
			{
				writer.Write(ch);
			}
		}

		written = writer.Written;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, PolybiusSquare key, out int written)
	{
		var writer = new SpanWriter<char>(plaintext);

		var lastIndex = ciphertext.Length - 1;
		for (int i = 0; i < lastIndex; i++)
		{
			if (!Int32.TryParse(ciphertext.Slice(i, 1), out var firstLabelIndex))
			{
				writer.Write(ciphertext[i]);
				continue;
			}

			if (!Int32.TryParse(ciphertext.Slice(i + 1, 1), out var secondLabelIndex))
			{
				writer.Write(ciphertext[i]);
				writer.Write(ciphertext[i + 1]);
				i++;
				continue;
			}

			writer.Write(key[firstLabelIndex - 1, secondLabelIndex - 1]);
			i++;
		}

		written = writer.Written;
	}
}
