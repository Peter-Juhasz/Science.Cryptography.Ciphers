using System;
using System.Buffers;
using System.Buffers.Text;
using System.Composition;
using System.Text;

namespace Science.Cryptography.Ciphers.Specialized;

[Export("Base64", typeof(ICipher))]
public class Base64Encoder : ICipher
{
	public Base64Encoder(Encoding encoding, Base64Options options)
	{
		Encoding = encoding;
		Options = options;
	}
	public Base64Encoder(Encoding encoding)
		: this(encoding, Base64Options.Default)
	{ }
	public Base64Encoder()
		: this(Encoding.UTF8, Base64Options.Default)
	{ }

	public Encoding Encoding { get; }
	public Base64Options Options { get; }

	public int MaxOutputCharactersPerInputCharacter => 4;

	private static readonly SearchValues<char> NonAlphanumericCharacters = SearchValues.Create(['+', '/', '=']);

	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written)
	{
		var count = Encoding.GetByteCount(plaintext);
		Span<byte> buffer = stackalloc byte[count];
		Encoding.GetBytes(plaintext, buffer);

		// url special case
		if (Options == Base64Options.Url)
		{
			Base64Url.EncodeToChars(buffer, ciphertext, out _, out written);
			return;
		}

		// regular case
		Convert.TryToBase64Chars(buffer, ciphertext, out written);

		// format
		if (Options != Base64Options.Default)
		{
			var options = Options;
			var writtenSpan = ciphertext[..written];
			while (writtenSpan.IndexOfAny(NonAlphanumericCharacters) is int index and > -1)
			{
				switch (writtenSpan[index])
				{
					case '+': writtenSpan[index] = options.Plus; break;
					case '/': writtenSpan[index] = options.Slash; break;
					case '=': writtenSpan[index] = options.Padding; break;
				}
			}
		}
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written)
	{
		Span<byte> buffer = stackalloc byte[ciphertext.Length];
		int bufferLength;

		// format
		if (Options != Base64Options.Default)
		{
			if (Options == Base64Options.Url)
			{
				Base64Url.DecodeFromChars(ciphertext, buffer, out _, out bufferLength);
			}
			else
			{
				var options = Options;
				Span<char> formatted = stackalloc char[ciphertext.Length];
				ciphertext.CopyTo(formatted);
				while (formatted.IndexOfAny(options.Plus, options.Slash, options.Padding) is int index and > -1)
				{
					var ch = formatted[index];
					if (ch == options.Plus)
					{
						formatted[index] = '+';
					}
					else if (ch == options.Slash)
					{
						formatted[index] = '/';
					}
					else if (ch == options.Padding)
					{
						formatted[index] = '=';
					}
				}
				Convert.TryFromBase64Chars(formatted, buffer, out bufferLength);
			}
		}
		else
		{
			Convert.TryFromBase64Chars(ciphertext, buffer, out bufferLength);
		}

		written = Encoding.GetChars(buffer[..bufferLength], plaintext);
	}
}
