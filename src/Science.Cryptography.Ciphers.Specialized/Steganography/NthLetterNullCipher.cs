using System;
using System.Composition;

namespace Science.Cryptography.Ciphers.Specialized;

[Export("NthLetter", typeof(ICipher))]
public sealed class NthLetterNullCipher : IKeyedCipher<NthCharacterKey>
{
	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, NthCharacterKey key, out int written)
	{
		throw new NotSupportedException();
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, NthCharacterKey key, out int written)
	{
		var writer = new SpanWriter<char>(plaintext);
		var letterIndex = 0;

		foreach (var ch in ciphertext)
		{
			if (Char.IsLetter(ch))
			{
				letterIndex++;
			}

			if ((letterIndex - key.Offset) % key.N == 0)
			{
				writer.Write(ch);
			}
		}

		written = writer.Written;
	}
}