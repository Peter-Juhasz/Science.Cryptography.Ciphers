using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the cipher named after Johann Franz Graf Gronsfeld-Bronkhorst.
/// </summary>
[Export("Gronsfeld", typeof(IKeyedCipher<>))]
public class GronsfeldCipher : IKeyedCipher<int[]>
{
	public GronsfeldCipher(Alphabet alphabet)
	{
		_inner = new(alphabet);
	}
	public GronsfeldCipher()
		: this(WellKnownAlphabets.English)
	{ }

	private readonly VigenèreCipher _inner;

	public Alphabet Alphabet => _inner.Alphabet;


	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, int[] key, out int written) => _inner.Encrypt(plaintext, ciphertext, GetVigenèreKey(key), out written);

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, int[] key, out int written) => _inner.Decrypt(ciphertext, plaintext, GetVigenèreKey(key), out written);


	private string GetVigenèreKey(int[] key) => String.Create<object>(key.Length, null, (span, state) =>
	{
		for (int i = 0; i < key.Length; i++)
		{
			span[i] = Alphabet[key[i]];
		}
	});
}
