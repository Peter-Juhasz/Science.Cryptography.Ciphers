using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Kama-Sutra cipher.
/// </summary>
[Export("Kamasutra", typeof(IKeyedCipher<>))]
public class KamaSutraCipher : ReciprocalKeyedCipher<CharacterSubstitutionMap>
{
    protected override void Crypt(ReadOnlySpan<char> input, Span<char> output, CharacterSubstitutionMap key, out int written)
    {
        for (int i = 0; i < input.Length; i++)
        {
            var ch = input[i];
            output[i] = key.LookupOrSame(ch);
        }

        written = input.Length;
    }
}
