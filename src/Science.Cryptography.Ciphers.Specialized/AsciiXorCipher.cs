using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace Science.Cryptography.Ciphers.Specialized;

/// <summary>
/// Represents the XOR cipher.
/// </summary>
[Export("XOR", typeof(IKeyedCipher<>))]
public class AsciiXorCipher : ReciprocalKeyedCipher<IReadOnlyList<byte>>
{
    public Encoding Encoding = Encoding.ASCII;

    protected override void Crypt(ReadOnlySpan<char> input, Span<char> output, IReadOnlyList<byte> key, out int written)
    {
        if (key.Count == 0)
        {
            throw new ArgumentException("Key can't be zero-length.", nameof(key));
        }

        if (output.Length < input.Length)
        {
            throw new ArgumentException("Size of output buffer is insufficient.", nameof(output));
        }

        // prepare buffer
        var length = input.Length;
        Span<byte> inputBytes = stackalloc byte[length];
        Encoding.ASCII.GetBytes(input, inputBytes);

        Span<byte> outputBytes = stackalloc byte[length];

        int keyLength = key.Count;
        Span<byte> keyBytes = stackalloc byte[keyLength];
        if (key is byte[] keyIsArray)
        {
            keyIsArray.CopyTo(keyBytes);
        }
        else
        {
            for (int i = 0; i < keyLength; i++)
            {
                keyBytes[i] = key[i];
            }
        }

        // crypt
        BinaryXor.Xor(inputBytes, outputBytes, keyBytes);

        // decode
        written = Encoding.GetChars(outputBytes, output);
    }
}
