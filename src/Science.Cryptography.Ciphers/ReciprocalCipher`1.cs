using System;

namespace Science.Cryptography.Ciphers;

public abstract class ReciprocalKeyedCipher<TKey> : IKeyedCipher<TKey>
{
    public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, TKey key, out int written) => Crypt(plaintext, ciphertext, key, out written);

    public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, TKey key, out int written) => Crypt(ciphertext, plaintext, key, out written);

    protected abstract void Crypt(ReadOnlySpan<char> input, Span<char> output, TKey key, out int written);
}
