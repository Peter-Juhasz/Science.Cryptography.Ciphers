using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the ROT-13 cipher.
/// </summary>
[Export("ROT-13", typeof(ICipher))]
public class Rot13Cipher : ICipher
{
    public Rot13Cipher(Alphabet alphabet)
    {
        _shift = new(alphabet);
    }

    private readonly ShiftCipher _shift;

    public Alphabet Alphabet => _shift.Alphabet;


    public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written) => _shift.Encrypt(plaintext, ciphertext, WellKnownShiftCipherKeys.Rot13, out written);

    public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written) => _shift.Decrypt(ciphertext, plaintext, WellKnownShiftCipherKeys.Rot13, out written);
}
