using System;
using System.Composition;

namespace Science.Cryptography.Ciphers.Specialized;

[Export("NthCharacter", typeof(ICipher))]
public sealed class NthCharacterNullCipher : IKeyedCipher<NthCharacterKey>
{
	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, NthCharacterKey key, out int written)
	{
		throw new NotSupportedException();
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, NthCharacterKey key, out int written)
	{
		var writer = new SpanWriter<char>(plaintext);

		for (int i = key.Offset; i < ciphertext.Length; i += key.N)
		{
			var ch = ciphertext[i];
			writer.Write(ch);
		}

		written = writer.Written;
	}
}
