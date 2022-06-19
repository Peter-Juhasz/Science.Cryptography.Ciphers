using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Science.Cryptography.Ciphers.Tests")]

namespace Science.Cryptography.Ciphers;

public interface ICipher
{
	void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written);

	void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written);

	[EditorBrowsable(EditorBrowsableState.Never)]
	int MaxOutputCharactersPerInputCharacter => 1;
}

public static partial class CipherExtensions
{
	public static string Encrypt(this ICipher cipher, string plaintext)
	{
		if (cipher.MaxOutputCharactersPerInputCharacter > 1)
		{
			Span<char> buffer = stackalloc char[plaintext.Length * cipher.MaxOutputCharactersPerInputCharacter];
			cipher.Encrypt(plaintext, buffer, out var written);
			return new string(buffer[..written]);
		}

		return String.Create<object>(plaintext.Length, null, (span, state) => cipher.Encrypt(plaintext, span, out _));
	}

	public static string Decrypt(this ICipher cipher, string ciphertext)
	{
		if (cipher.MaxOutputCharactersPerInputCharacter > 1)
		{
			Span<char> buffer = stackalloc char[ciphertext.Length * cipher.MaxOutputCharactersPerInputCharacter];
			cipher.Decrypt(ciphertext, buffer, out var written);
			return new string(buffer[..written]);
		}

		return String.Create<object>(ciphertext.Length, null, (span, state) => cipher.Decrypt(ciphertext, span, out _));
	}
}