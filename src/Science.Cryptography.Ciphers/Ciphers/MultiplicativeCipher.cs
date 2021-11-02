using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Multiplicative cipher.
/// </summary>
[Export("Multiplicative", typeof(IKeyedCipher<>))]
public class MultiplicativeCipher : IKeyedCipher<int>
{
    public MultiplicativeCipher(Alphabet alphabet)
    {
        Alphabet = alphabet;
    }
    public MultiplicativeCipher()
        : this(WellKnownAlphabets.English)
    { }

    public Alphabet Alphabet { get; }

    public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, int key, out int written)
    {
        for (int i = 0; i < plaintext.Length; i++)
        {
            var ch = plaintext[i];
            ciphertext[i] = Alphabet.IndexOfIgnoreCase(ch) switch
            {
                -1 => ch,
                int idx => Alphabet.AtMod(idx * key).ToSameCaseAs(ch)
            };
        }

        written = plaintext.Length;
    }

    public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, int key, out int written)
    {
        throw new NotSupportedException();
    }
}
