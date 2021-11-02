using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Shift cipher.
/// </summary>
[Export("Shift", typeof(IKeyedCipher<>))]
public class ShiftCipher : IKeyedCipher<int>
{
    public ShiftCipher(Alphabet charset)
    {
        Alphabet = charset;
    }
    public ShiftCipher()
        : this(WellKnownAlphabets.English)
    { }

    public Alphabet Alphabet { get; }

    protected void Crypt(ReadOnlySpan<char> text, Span<char> result, int key, out int written)
    {
        if (result.Length < text.Length)
        {
            throw new ArgumentException("Size of output buffer is insufficient.", nameof(result));
        }

        for (int i = 0; i < text.Length; i++)
        {
            var ch = text[i];
            result[i] = Alphabet.IndexOfIgnoreCase(ch) switch
            {
                -1 => ch,
                int idx => Alphabet.AtMod(idx + key).ToSameCaseAs(ch)
            };
        }

        written = text.Length;
    }

    public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, int key, out int written) => Crypt(plaintext, ciphertext, key, out written);

    public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, int key, out int written) => Crypt(ciphertext, plaintext, -key, out written);
}
