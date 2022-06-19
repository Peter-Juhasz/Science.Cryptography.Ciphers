using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents Blaise de Vigenère's cipher.
/// </summary>
[Export("Vigenère", typeof(IKeyedCipher<>))]
public class VigenèreCipher : IKeyedCipher<char[]>
{
	public VigenèreCipher(Alphabet alphabet)
	{
		this.Alphabet = alphabet;
		_tabulaRecta = new TabulaRecta(this.Alphabet);
	}
	public VigenèreCipher()
		: this(WellKnownAlphabets.English)
	{ }

	private readonly TabulaRecta _tabulaRecta;

	public Alphabet Alphabet { get; }


	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, char[] key, out int written)
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
				_ => _tabulaRecta[key[charCounter++ % key.Length], ch].ToSameCaseAs(ch)
			};
		}

		written = plaintext.Length;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, char[] key, out int written)
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
				int idx => Alphabet.AtMod(idx - Alphabet.IndexOfIgnoreCase(key[charCounter++ % key.Length])).ToSameCaseAs(ch)
			};
		}

		written = ciphertext.Length;
	}
}
