using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents Julis Caesar's cipher.
/// </summary>
[Export("Caesar", typeof(ICipher))]
public class CaesarCipher : ICipher
{
	public CaesarCipher(Alphabet alphabet)
	{
		_shift = new(alphabet);
	}
	public CaesarCipher()
		: this(WellKnownAlphabets.English)
	{ }

	private readonly ShiftCipher _shift;

	public Alphabet Alphabet => _shift.Alphabet;


	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written) => _shift.Encrypt(plaintext, ciphertext, WellKnownShiftCipherKeys.Caesar, out written);

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written) => _shift.Decrypt(ciphertext, plaintext, WellKnownShiftCipherKeys.Caesar, out written);
}
