using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents Johannes Trithemius' cipher.
/// </summary>
[Export("Trithemius", typeof(ICipher))]
public class TrithemiusCipher : ICipher
{
	public TrithemiusCipher(Alphabet alphabet)
	{
		Alphabet = alphabet;
		_inner = new(alphabet);
		_innerKey = alphabet.ToCharArray();
	}
	public TrithemiusCipher()
		: this(WellKnownAlphabets.English)
	{ }

	private readonly VigenèreCipher _inner;
	private readonly char[] _innerKey;

	public Alphabet Alphabet { get; }


	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written) => _inner.Encrypt(plaintext, ciphertext, _innerKey, out written);

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written) => _inner.Decrypt(ciphertext, plaintext, _innerKey, out written);
}
