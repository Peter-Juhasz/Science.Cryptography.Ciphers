using System;
using System.Composition;
using System.Text;

namespace Science.Cryptography.Ciphers.Specialized;

[Export("ASCII-Binary", typeof(ICipher))]
public class AsciiBinaryBase : ICipher
{
	public AsciiBinaryBase(BinaryOptions options)
	{
		Options = options;
	}
	public AsciiBinaryBase()
		: this(BinaryOptions.Default)
	{ }

	private static readonly Encoding Encoding = Encoding.ASCII;
	public BinaryOptions Options { get; }

	public int MaxOutputCharactersPerInputCharacter => 8;

	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written)
	{
		int i = 0;
		var (zero, one) = Options;
		foreach (var @byte in plaintext)
		{
			var value = (int)@byte;
			ciphertext[i++] = ((value & 0b10000000) == 0b10000000) ? one : zero;
			ciphertext[i++] = ((value & 0b01000000) == 0b01000000) ? one : zero;
			ciphertext[i++] = ((value & 0b00100000) == 0b00100000) ? one : zero;
			ciphertext[i++] = ((value & 0b00010000) == 0b00010000) ? one : zero;
			ciphertext[i++] = ((value & 0b00001000) == 0b00001000) ? one : zero;
			ciphertext[i++] = ((value & 0b00000100) == 0b00000100) ? one : zero;
			ciphertext[i++] = ((value & 0b00000010) == 0b00000010) ? one : zero;
			ciphertext[i++] = ((value & 0b00000001) == 0b00000001) ? one : zero;
		}
		written = i;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written)
	{
		int start = 0, end = 0;
		var (zero, one) = Options;

		var writer = new SpanWriter<char>(plaintext);

		for (int i = 0; i < ciphertext.Length; i++)
		{
			var ch = ciphertext[i];
			if (ch == zero || ch == one)
			{
				if (start == end)
				{
					start = i;
				}

				end = i + 1;

				if (end - start == 8)
				{
					int value = 0;
					if (ciphertext[start + 0] == one) value += 0b10000000;
					if (ciphertext[start + 1] == one) value += 0b01000000;
					if (ciphertext[start + 2] == one) value += 0b00100000;
					if (ciphertext[start + 3] == one) value += 0b00010000;
					if (ciphertext[start + 4] == one) value += 0b00001000;
					if (ciphertext[start + 5] == one) value += 0b00000100;
					if (ciphertext[start + 6] == one) value += 0b00000010;
					if (ciphertext[start + 7] == one) value += 0b00000001;
					writer.Write((char)value);
					start = end = 0;
				}
			}
			else
			{
				writer.Write(ch);
				start = end = 0;
			}
		}

		written = writer.Written;
	}
}
