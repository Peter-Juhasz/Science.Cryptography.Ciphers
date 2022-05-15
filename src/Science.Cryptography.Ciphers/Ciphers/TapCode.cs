using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Tap Code cipher.
/// </summary>
[Export("Tap Code", typeof(ICipher))]
public class TapCode : ICipher, IKeyedCipher<PolybiusSquare>
{
	public TapCode(TapCodeOptions options)
	{
		Options = options;
	}
	public TapCode()
		: this(TapCodeOptions.Default)
	{ }

	public int MaxOutputCharactersPerInputCharacter => 5 + 5 + 1;

	public TapCodeOptions Options { get; }

	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written) => Encrypt(plaintext, ciphertext, PolybiusSquare.RegularWithoutK, out written);
	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, PolybiusSquare key, out int written)
	{
		bool writeDelimiter = false;

		char delimiter = Options.Delimiter;
		char dot = Options.Dot;

		var writer = new SpanWriter<char>(ciphertext);

		foreach (var ch in plaintext)
		{
			if (key.TryFindOffsets(ch, out (int row, int column) position))
			{
				if (writeDelimiter)
				{
					writer.Write(delimiter);
				}

				writer.GetSpan(position.row + 1).Fill(dot);
				writer.Write(delimiter);
				writer.GetSpan(position.column + 1).Fill(dot);
				writeDelimiter = true;
			}
		}

		written = writer.Written;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written) => Decrypt(ciphertext, plaintext, PolybiusSquare.RegularWithoutK, out written);
	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, PolybiusSquare key, out int written)
	{
		int row = 0, column = 0;
		int values = 0;

		char delimiter = Options.Delimiter;
		char dot = Options.Dot;

		var writer = new SpanWriter<char>(plaintext);

		foreach (char c in ciphertext)
		{
			// meta character
			if (c == dot)
			{
				if (values == 0)
					row++;
				else if (values == 1)
					column++;
			}

			// every other characters
			else if (c == delimiter)
			{
				// step dimension
				values++;

				// if both dimensions are ready
				if (values == 2)
				{
					if (row != 0 && column != 0)
					{
						writer.Write(key[row - 1, column - 1]);
					}

					row = column = 0;
					values = 0;
				}
			}
		}

		if (row != 0 && column != 0)
			writer.Write(key[row - 1, column - 1]);

		written = writer.Written;
	}
}
