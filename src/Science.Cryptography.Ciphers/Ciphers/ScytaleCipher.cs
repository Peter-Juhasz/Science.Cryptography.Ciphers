using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

[Export("Scytale", typeof(IKeyedCipher<>))]
public class ScytaleCipher : IKeyedCipher<int>
{
	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, int key, out int written)
	{
		var writer = new SpanWriter<char>(ciphertext);

		var rows = key;
		var columns = plaintext.Length / key;

		for (var column = 0; column < columns; column++)
		{
			for (var row = 0; row < rows; row++)
			{
				int index = column + row * columns;
				if (index < plaintext.Length)
				{
					writer.Write(plaintext[index]);
				}
			}
		}

		written = writer.Written;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, int key, out int written)
	{
		var writer = new SpanWriter<char>(plaintext);

		var rows = key;
		var columns = ciphertext.Length / key;

		for (var column = 0; column < rows; column++)
		{
			for (var row = 0; row < columns; row++)
			{
				int index = column + row * rows;
				if (index < ciphertext.Length)
				{
					writer.Write(ciphertext[index]);
				}
			}
		}

		written = writer.Written;
	}
}
