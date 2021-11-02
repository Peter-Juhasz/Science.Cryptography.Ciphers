using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents Francis Bacon's cipher.
/// </summary>
[Export("Bacon", typeof(ICipher))]
public class BaconCipher : ICipher
{
    public BaconCipher(BaconOptions options)
    {
        Options = options;
        _map = options.GetSubstitutionMap();
    }
    public BaconCipher()
        : this(BaconOptions.Default)
    { }

    private readonly CharacterToSegmentSubstitutionMap _map;

    public int MaxOutputCharactersPerInputCharacter => 5;

    public BaconOptions Options { get; }

    public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written)
    {
        var writtenPosition = 0;

        foreach (var ch in plaintext)
        {
            if (!_map.TryLookup(ch, ciphertext[writtenPosition..], ref writtenPosition))
            {
                ciphertext[writtenPosition++] = ch;
            }
        }

        written = writtenPosition;
    }

    public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written)
    {
        var A = Options.A;
        var B = Options.B;

        int start = 0, end = 0;

        var writtenPosition = 0;

        for (int i = 0; i < ciphertext.Length; i++)
        {
            var ch = ciphertext[i];
            if (ch == A || ch == B)
            {
                if (end - start == 4)
                {
                    end = i + 1;
                    if (_map.TryReverseLookup(ciphertext[start..end], out var tr))
                    {
                        plaintext[writtenPosition++] = tr;
                    }
                    else
                    {
                        plaintext[writtenPosition++] = ch;
                    }
                    start = end = 0;
                }
                else if (start < end)
                {
                    end = i + 1;
                }
                else if (start == end)
                {
                    start = i;
                    end = i + 1;
                }
            }
            else
            {
                plaintext[writtenPosition++] = ch;
                start = end = 0;
            }
        }

        written = writtenPosition;
    }
}
