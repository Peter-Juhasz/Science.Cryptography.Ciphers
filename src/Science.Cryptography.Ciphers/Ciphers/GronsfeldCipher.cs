using System;
using System.Collections.Generic;
using System.Composition;

namespace Science.Cryptography.Ciphers;

using GronsfeldKey = IReadOnlyList<int>;

/// <summary>
/// Represents the cipher named after Johann Franz Graf Gronsfeld-Bronkhorst.
/// </summary>
[Export("Gronsfeld", typeof(IKeyedCipher<>))]
public class GronsfeldCipher : IKeyedCipher<GronsfeldKey>
{
    public GronsfeldCipher(Alphabet alphabet)
    {
        _inner = new(alphabet);
    }
    public GronsfeldCipher()
        : this(WellKnownAlphabets.English)
    { }

    private readonly VigenèreCipher _inner;

    public Alphabet Alphabet => _inner.Alphabet;


    public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, GronsfeldKey key, out int written) => _inner.Encrypt(plaintext, ciphertext, GetVigenèreKey(key), out written);

    public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, GronsfeldKey key, out int written) => _inner.Decrypt(ciphertext, plaintext, GetVigenèreKey(key), out written);


    private string GetVigenèreKey(GronsfeldKey key) => String.Create<object>(key.Count, null, (span, state) =>
    {
        for (int i = 0; i < key.Count; i++)
        {
            span[i] = Alphabet[key[i]];
        }
    });
}
