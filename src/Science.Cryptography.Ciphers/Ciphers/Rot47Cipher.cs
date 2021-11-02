using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the ROT-47 cipher.
/// </summary>
[Export("ROT-47", typeof(ICipher))]
public class Rot47Cipher : ReciprocalCipher
{
    protected override void Crypt(ReadOnlySpan<char> text, Span<char> result, out int written)
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == ' ')
            {
                result[i] = ' ';
                continue;
            }

            int ascii = text[i] + 47;

            if (ascii > 126)
                ascii -= 94;
            if (ascii < 33)
                ascii += 94;

            result[i] = (char)ascii;
        }

        written = text.Length;
    }
}
