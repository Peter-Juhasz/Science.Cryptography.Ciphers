using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the One-Time Pad technique.
/// </summary>
[Export("One-Time Pad", typeof(IKeyedCipher<>))]
[Export("OTP", typeof(IKeyedCipher<>))]
public class OneTimePadCipher : IKeyedCipher<string>
{
	public OneTimePadCipher(Alphabet alphabet)
	{
		this.Alphabet = alphabet;
	}
	public OneTimePadCipher()
		: this(WellKnownAlphabets.English)
	{ }

	public Alphabet Alphabet { get; }


	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, string key, out int written)
	{
		if (ciphertext.Length < plaintext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(ciphertext));
		}

		int charCounter = 0;

		for (int i = 0; i < plaintext.Length; i++)
		{
			var ch = plaintext[i];
			ciphertext[i] = Alphabet.IndexOfIgnoreCase(ch) switch
			{
				-1 => ch,
				int idx => Alphabet.AtMod(idx + Alphabet.IndexOfIgnoreCase(key[charCounter++])).ToSameCaseAs(ch)
			};
		}

		written = plaintext.Length;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, string key, out int written)
	{
		if (plaintext.Length < ciphertext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(plaintext));
		}

		int charCounter = 0;

		for (int i = 0; i < ciphertext.Length; i++)
		{
			var ch = ciphertext[i];
			plaintext[i] = Alphabet.IndexOfIgnoreCase(ch) switch
			{
				-1 => ch,
				int idx => Alphabet.AtMod(idx - Alphabet.IndexOfIgnoreCase(key[charCounter++])).ToSameCaseAs(ch)
			};
		}

		written = ciphertext.Length;
	}
}
