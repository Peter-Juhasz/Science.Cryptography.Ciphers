using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the A1Z26 cipher
/// </summary>
[Export("A1Z26", typeof(ICipher))]
public class A1Z26Cipher : ICipher
{
	public A1Z26Cipher(Alphabet charset, A1Z26Options options)
	{
		Alphabet = charset;
		Options = options;
	}
	public A1Z26Cipher()
		: this(WellKnownAlphabets.English, A1Z26Options.Default)
	{ }

	public Alphabet Alphabet { get; }
	public A1Z26Options Options { get; }

	public int MaxOutputCharactersPerInputCharacter => GetDecimalLength(Alphabet.Length) + 1;

	private static int GetDecimalLength(int @decimal) => @decimal switch
	{
		>= 1000 => throw new NotSupportedException(),
		>= 100 => 3,
		>= 10 => 2,
		>= 0 => 1,
		_ => throw new ArgumentOutOfRangeException(nameof(@decimal))
	};

	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written)
	{
		if (ciphertext.Length < plaintext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(ciphertext));
		}

		var addSeparator = false;
		var separator = Options.Separator;
		var writer = new SpanWriter<char>(ciphertext);

		foreach (var ch in plaintext)
		{
			var idx = Alphabet.IndexOfIgnoreCase(ch);
			if (idx == -1)
			{
				writer.Write(ch);
				addSeparator = false;
			}
			else
			{
				if (addSeparator)
				{
					writer.Write(separator);
				}
				var num = idx + 1;
				var succeeded = num.TryFormat(writer.GetSpan(GetDecimalLength(num)), out _);
				if (!succeeded)
				{
					throw new ArgumentException("Size of output buffer is insufficient.", nameof(ciphertext));
				}
				addSeparator = true;
			}
		}

		written = writer.Written;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written)
	{
		if (plaintext.Length < ciphertext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(plaintext));
		}

		int start = 0, end = 0;

		var writer = new SpanWriter<char>(plaintext);

		for (int i = 0; i < ciphertext.Length; i++)
		{
			var ch = ciphertext[i];
			if (ch is >= '0' and <= '9')
			{
				if (start == end)
				{
					start = i;
				}

				end = i + 1;
			}
			else
			{
				if (start < end)
				{
					var span = ciphertext[start..end];
					var number = Int32.Parse(span);
					var result = Alphabet[number - 1];
					writer.Write(result);
				}

				if (ch != Options.Separator)
				{
					writer.Write(ch);
				}

				start = end = 0;
			}
		}

		if (start < end)
		{
			var span = ciphertext[start..end];
			var number = Int32.Parse(span);
			var result = Alphabet[number - 1];
			writer.Write(result);
		}

		written = writer.Written;
	}
}
