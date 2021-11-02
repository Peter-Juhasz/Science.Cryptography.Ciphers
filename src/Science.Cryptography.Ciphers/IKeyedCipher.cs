using System;
using System.ComponentModel;

namespace Science.Cryptography.Ciphers;

public interface IKeyedCipher<TKey>
{
    void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, TKey key, out int written);

    void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, TKey key, out int written);

    [EditorBrowsable(EditorBrowsableState.Never)]
    int GetMaxOutputCharactersPerInputCharacter(TKey key) => 1;
}

public static partial class CipherExtensions
{
    public static string Encrypt<TKey>(this IKeyedCipher<TKey> cipher, string plaintext, TKey key)
    {
        var maxLength = cipher.GetMaxOutputCharactersPerInputCharacter(key);
        if (maxLength > 1)
        {
            Span<char> buffer = stackalloc char[plaintext.Length * maxLength];
            cipher.Encrypt(plaintext, buffer, key, out var written);
            return new string(buffer[..written]);
        }

        return String.Create<object>(plaintext.Length, null, (span, state) => cipher.Encrypt(plaintext, span, key, out _));
    }

    public static string Decrypt<TKey>(this IKeyedCipher<TKey> cipher, string ciphertext, TKey key)
    {
        var maxLength = cipher.GetMaxOutputCharactersPerInputCharacter(key);
        if (maxLength > 1)
        {
            Span<char> buffer = stackalloc char[ciphertext.Length * maxLength];
            cipher.Decrypt(ciphertext, buffer, key, out var written);
            return new string(buffer[..written]);
        }

        return String.Create<object>(ciphertext.Length, null, (span, state) => cipher.Decrypt(ciphertext, span, key, out _));
    }

    /// <summary>
    /// Pins a key to a <see cref="IKeyedCipher{TKey}"/> and converts it to a <see cref="ICipher"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the key of <paramref name="keyedCipher"/>.</typeparam>
    /// <param name="keyedCipher">The cipher to use.</param>
    /// <param name="key">The key to use.</param>
    /// <returns>A <see cref="ICipher"/> which operates with the provided fix key.</returns>
    public static ICipher Pin<TKey>(this IKeyedCipher<TKey> keyedCipher, TKey key)
    {
        if (keyedCipher == null)
            throw new ArgumentNullException(nameof(keyedCipher));

        return new PinnedKeyCipher<TKey>(keyedCipher, key);
    }

    private sealed class PinnedKeyCipher<TKey> : ICipher
    {
        public PinnedKeyCipher(IKeyedCipher<TKey> keyedCipher, TKey key)
        {
            this.Cipher = keyedCipher;
            this.Key = key;
        }

        public IKeyedCipher<TKey> Cipher { get; }

        public TKey Key { get; }

        public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written) => Cipher.Encrypt(plaintext, ciphertext, Key, out written);

        public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written) => Cipher.Decrypt(ciphertext, plaintext, Key, out written);
    }
}
