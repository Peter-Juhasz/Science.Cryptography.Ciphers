using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Null cipher.
/// </summary>
[Export("Gudhayojya", typeof(IKeyedCipher<>))]
public class GudhayojyaCipher : IKeyedCipher<string>
{
    public int GetMaxOutputCharactersPerInputCharacter(string key) => key.Length + 1;

    public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, string key, out int written)
    {
        var writer = new SpanWriter<char>(ciphertext);

        if (plaintext[0].IsLetter())
        {
            writer.Write(key);
        }

        writer.Write(plaintext[0]);

        for (int i = 1; i < plaintext.Length; i++)
        {
            if (!plaintext[i - 1].IsLetter() && plaintext[i].IsLetter())
            {
                writer.Write(key);
            }

            writer.Write(plaintext[i]);
        }

        written = writer.Written;
    }

    public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, string key, out int written)
    {
        var writer = new SpanWriter<char>(plaintext);

        for (int i = 0; i < ciphertext.Length; i++)
        {
            if ((i == 0 || !ciphertext[i - 1].IsLetter()) && ciphertext[i].IsLetter() &&
                i + key.Length < ciphertext.Length &&
                ciphertext.Slice(i, key.Length).Equals(key, StringComparison.OrdinalIgnoreCase)
            )
            {
                i += key.Length - 1;
                continue;
            }

            writer.Write(ciphertext[i]);
        }

        written = writer.Written;
    }
}
