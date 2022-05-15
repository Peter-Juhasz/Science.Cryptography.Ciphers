using System;
using System.Composition;

namespace Science.Cryptography.Ciphers.Specialized;

[Export("NthLetterOfWords", typeof(ICipher))]
public sealed class NthLetterOfWordsNullCipher : IKeyedCipher<int>
{
	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, int key, out int written)
	{
		throw new NotSupportedException();
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, int key, out int written)
	{
		var writer = new SpanWriter<char>(plaintext);

		int wordStart = -1;

		for (int i = 0; i < ciphertext.Length; i++)
		{
			var ch = ciphertext[i];
			if (Char.IsLetter(ch))
			{
				if (wordStart == -1)
				{
					wordStart = i;
				}

				if (i - wordStart == key)
				{
					writer.Write(ch);
				}
			}
			else
			{
				wordStart = -1;
			}
		}

		written = writer.Written;
	}
}
