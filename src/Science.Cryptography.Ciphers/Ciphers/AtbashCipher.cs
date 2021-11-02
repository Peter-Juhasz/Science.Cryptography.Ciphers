using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Atbash cipher.
/// </summary>
[Export("Atbash", typeof(ICipher))]
public class AtbashCipher : ReciprocalCipher
{
    public AtbashCipher(Alphabet charset)
    {
        Alphabet = charset;
    }
    public AtbashCipher()
        : this(WellKnownAlphabets.English)
    { }

    public Alphabet Alphabet { get; }

    protected override void Crypt(ReadOnlySpan<char> text, Span<char> result, out int written)
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
                int idx => Alphabet[Alphabet.Length - idx - 1].ToSameCaseAs(ch)
            };
        }

        written = text.Length;
    }
}
