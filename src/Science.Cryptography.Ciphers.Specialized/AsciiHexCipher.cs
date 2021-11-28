using System;
using System.Composition;
using System.Text;

namespace Science.Cryptography.Ciphers.Specialized;

[Export("Hex", typeof(ICipher))]
public class AsciiHexCipher : ICipher
{
    public Encoding Encoding = Encoding.ASCII;

    public int MaxOutputCharactersPerInputCharacter => 2;

    public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written)
    {
        var count = Encoding.GetByteCount(plaintext);
        Span<byte> buffer = stackalloc byte[count];
        Encoding.GetBytes(plaintext, buffer);
        var result = Convert.ToHexString(buffer); // TODO: optimize string allocation
        result.CopyTo(ciphertext);
        written = result.Length;
    }

    public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written)
    {
        int start = 0, end = 0;

        var writtenPosition = 0;

        for (int i = 0; i < ciphertext.Length; i++)
        {
            var ch = ciphertext[i];
            if (ch is (>= 'a' and <= 'f') or (>= '0' and <= '9') or (>= 'A' and <= 'F'))
            {
                if (start == end)
                {
                    start = i;
                }

                end = i + 1;
            }
            else
            {
                if (start < end)
                {
                    var span = ciphertext[start..end];
                    var bytes = Convert.FromHexString(span);
                    writtenPosition += Encoding.GetChars(bytes, plaintext);
                }

                plaintext[writtenPosition++] = ch;
                start = end = 0;
            }
        }

        if (start < end)
        {
            var span = ciphertext[start..end];
            var bytes = Convert.FromHexString(span);
            writtenPosition += Encoding.GetChars(bytes, plaintext);
        }

        written = writtenPosition;
    }
}
