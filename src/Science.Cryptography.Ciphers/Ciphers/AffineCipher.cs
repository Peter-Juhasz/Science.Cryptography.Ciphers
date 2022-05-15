using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Affine cipher
/// </summary>
[Export("Affine", typeof(IKeyedCipher<>))]
public class AffineCipher : IKeyedCipher<AffineKey>
{
	public AffineCipher(Alphabet charset)
	{
		Alphabet = charset;
	}
	public AffineCipher()
		: this(WellKnownAlphabets.English)
	{ }

	public Alphabet Alphabet { get; }


	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, AffineKey key, out int written)
	{
		if (ciphertext.Length < plaintext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(ciphertext));
		}

		for (int i = 0; i < plaintext.Length; i++)
		{
			var ch = plaintext[i];
			ciphertext[i] = Alphabet.IndexOfIgnoreCase(ch) switch
			{
				-1 => ch,
				int idx => Alphabet.AtMod(key.Value(idx)).ToSameCaseAs(ch)
			};
		}

		written = plaintext.Length;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, AffineKey key, out int written)
	{
		if (plaintext.Length < ciphertext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(plaintext));
		}

		for (int i = 0; i < ciphertext.Length; i++)
		{
			var ch = ciphertext[i];
			plaintext[i] = Alphabet.IndexOfIgnoreCase(ch) switch
			{
				-1 => ch,
				int idx => Alphabet.AtMod((Alphabet.Length - key.A) * (idx - key.B)).ToSameCaseAs(ch)
			};
		}

		written = ciphertext.Length;
	}
}
