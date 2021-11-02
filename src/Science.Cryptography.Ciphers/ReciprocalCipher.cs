using System;

namespace Science.Cryptography.Ciphers;

public abstract class ReciprocalCipher : ICipher
{
    public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written) => Crypt(plaintext, ciphertext, out written);

    public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written) => Crypt(ciphertext, plaintext, out written);

    protected abstract void Crypt(ReadOnlySpan<char> input, Span<char> output, out int written);
}
