using System;
using System.Composition;
using System.Text;

namespace Science.Cryptography.Ciphers.Specialized;

[Export("Hex", typeof(ICipher))]
public class HexEncoder : ICipher
{
	public HexEncoder(Encoding encoding)
	{
		Encoding = encoding;
	}
	public HexEncoder()
		: this(Encoding.UTF8)
	{ }

	public Encoding Encoding { get; }

	public int MaxOutputCharactersPerInputCharacter => Encoding.GetMaxByteCount(1) * 2;

	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written)
	{
		var count = Encoding.GetByteCount(plaintext);
		Span<byte> buffer = stackalloc byte[count];
		Encoding.GetBytes(plaintext, buffer);
		var hexCiphertext = ciphertext[..(count * 2)]; // `TryToHexString` requires exact length
		Convert.TryToHexString(buffer, hexCiphertext, out written);
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written)
	{
		int start = 0, end = 0;

		var writtenPosition = 0;

		for (int i = 0; i < ciphertext.Length; i++)
		{
			var ch = ciphertext[i];
			if (ch is (>= 'a' and <= 'f') or (>= '0' and <= '9') or (>= 'A' and <= 'F'))
			{
				if (start == end)
				{
					start = i;
				}

				end = i + 1;
			}
			else
			{
				if (start < end && (end - start) % 2 == 0)
				{
					var span = ciphertext[start..end];
					var bytesLength = span.Length / 2;
					Span<byte> bytesBuffer = bytesLength < 1024 ? stackalloc byte[bytesLength] : new byte[bytesLength];
					Convert.FromHexString(span, bytesBuffer, out _, out _);
					writtenPosition += Encoding.GetChars(bytesBuffer, plaintext);
				}

				plaintext[writtenPosition++] = ch;
				start = end = 0;
			}
		}

		if (start < end && (end - start) % 2 == 0)
		{
			var span = ciphertext[start..end];
			var bytesLength = span.Length / 2;
			Span<byte> bytesBuffer = bytesLength < 1024 ? stackalloc byte[bytesLength] : new byte[bytesLength];
			Convert.FromHexString(span, bytesBuffer, out _, out _);
			writtenPosition += Encoding.GetChars(bytesBuffer, plaintext);
		}

		written = writtenPosition;
	}
}
