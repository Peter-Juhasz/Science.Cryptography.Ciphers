using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Variant Beaufort.
/// </summary>
[Export("Variant Beaufort", typeof(IKeyedCipher<>))]
public class VariantBeaufortCipher : IKeyedCipher<string>
{
	public VariantBeaufortCipher(Alphabet alphabet)
	{
		_inner = new(alphabet);
	}
	public VariantBeaufortCipher()
		: this(WellKnownAlphabets.English)
	{ }

	private readonly VigenèreCipher _inner;

	public Alphabet Alphabet => _inner.Alphabet;


	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, string key, out int written) => _inner.Decrypt(plaintext, ciphertext, key, out written);

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, string key, out int written) => _inner.Encrypt(ciphertext, plaintext, key, out written);
}
