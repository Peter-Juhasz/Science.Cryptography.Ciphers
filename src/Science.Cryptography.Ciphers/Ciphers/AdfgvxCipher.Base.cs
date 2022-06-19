using System;

namespace Science.Cryptography.Ciphers;

public abstract class AdfgvxBaseCipher : IKeyedCipher<(PolybiusSquare polybiusSquare, int[] transpositionKey)>
{
	public AdfgvxBaseCipher(AdfgvxCipherOptions options)
	{
		Options = options;
	}

	private readonly ColumnarTranspositionCipher _transpositionCipher = new();

	public AdfgvxCipherOptions Options { get; }

	public int GetMaxOutputCharactersPerInputCharacter((PolybiusSquare polybiusSquare, int[] transpositionKey) key) => 2;

	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, (PolybiusSquare polybiusSquare, int[] transpositionKey) key, out int written)
	{
		if (ciphertext.Length < plaintext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(ciphertext));
		}

		var labels = Options.Labels;
		if (key.polybiusSquare.Size != labels.Length)
		{
			throw new ArgumentException($"The required size of {nameof(PolybiusSquare)} is {labels.Length}.", nameof(key));
		}

		Span<char> intermediate = stackalloc char[plaintext.Length * 2];
		var writer = new SpanWriter<char>(intermediate);

		foreach (var ch in plaintext)
		{
			if (key.polybiusSquare.TryFindOffsets(ch, out (int row, int column) position))
			{
				writer.Write(labels[position.row]);
				writer.Write(labels[position.column]);
			}
			else
			{
				writer.Write(ch);
			}
		}

		_transpositionCipher.Encrypt(intermediate[..writer.Written], ciphertext, key.transpositionKey, out written);
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, (PolybiusSquare polybiusSquare, int[] transpositionKey) key, out int written)
	{
		if (plaintext.Length < ciphertext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(plaintext));
		}

		var labels = Options.Labels.AsSpan();
		if (key.polybiusSquare.Size != labels.Length)
		{
			throw new ArgumentException($"The required size of {nameof(PolybiusSquare)} is {labels.Length}.", nameof(key));
		}

		Span<char> intermediate = stackalloc char[ciphertext.Length];
		_transpositionCipher.Decrypt(ciphertext, intermediate, key.transpositionKey, out written);

		var writer = new SpanWriter<char>(plaintext);

		var lastIndex = written - 1;
		for (int i = 0; i < lastIndex; i++)
		{
			var first = intermediate[i];
			var second = intermediate[i + 1];

			var firstLabelIndex = labels.IndexOf(first);
			if (firstLabelIndex == -1)
			{
				writer.Write(first);
				continue;
			}

			var secondLabelIndex = labels.IndexOf(second);
			if (secondLabelIndex == -1)
			{
				writer.Write(first);
				writer.Write(second);
				i++;
				continue;
			}

			writer.Write(key.polybiusSquare[firstLabelIndex, secondLabelIndex]);
			i++;
		}

		written = writer.Written;
	}
}
