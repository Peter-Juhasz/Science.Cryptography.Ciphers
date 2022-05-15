using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Autokey cipher.
/// </summary>
[Export("Autokey", typeof(IKeyedCipher<>))]
public class AutokeyCipher : IKeyedCipher<string>
{
	public AutokeyCipher(Alphabet charset)
	{
		Alphabet = charset;
	}
	public AutokeyCipher()
		: this(WellKnownAlphabets.English)
	{ }

	public Alphabet Alphabet { get; }


	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, string key, out int written)
	{
		if (ciphertext.Length < plaintext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(ciphertext));
		}

		int charCounter = -1;
		for (int i = 0; i < plaintext.Length; i++)
		{
			var ch = plaintext[i];
			if (Alphabet.IndexOfIgnoreCase(ch) is int idx and > -1)
			{
				charCounter++;
				if (charCounter < key.Length)
				{
					ciphertext[i] = Alphabet.AtMod(idx + Alphabet.IndexOfIgnoreCase(key[charCounter])).ToSameCaseAs(ch);
				}
				else
				{
					ciphertext[i] = Alphabet.AtMod(idx + Alphabet.IndexOfIgnoreCase(plaintext[charCounter - key.Length])).ToSameCaseAs(ch);
				}
			}
			else
			{
				ciphertext[i] = ch;
			}
		}

		written = plaintext.Length;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, string key, out int written)
	{
		if (plaintext.Length < ciphertext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(plaintext));
		}

		int charCounter = -1;
		for (int i = 0; i < ciphertext.Length; i++)
		{
			var ch = ciphertext[i];
			if (Alphabet.IndexOfIgnoreCase(ch) is int idx and > -1)
			{
				charCounter++;
				if (charCounter < key.Length)
				{
					plaintext[i] = Alphabet.AtMod(idx - Alphabet.IndexOfIgnoreCase(key[charCounter])).ToSameCaseAs(ch);
				}
				else
				{
					plaintext[i] = Alphabet.AtMod(idx - Alphabet.IndexOfIgnoreCase(plaintext[charCounter - key.Length])).ToSameCaseAs(ch);
				}
			}
			else
			{
				plaintext[i] = ch;
			}
		}

		written = ciphertext.Length;
	}
}