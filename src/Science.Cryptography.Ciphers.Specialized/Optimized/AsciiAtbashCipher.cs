using System;
using System.Composition;

namespace Science.Cryptography.Ciphers.Specialized;

/// <summary>
/// Represents the Atbash cipher.
/// </summary>
[Export("ASCII-Atbash", typeof(ICipher))]
public class AsciiAtbashCipher : ReciprocalCipher
{
	private const int A = (int)'A';
	private const int a = (int)'a';

	protected override void Crypt(ReadOnlySpan<char> text, Span<char> result, out int written)
	{
		if (result.Length < text.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(result));
		}

		for (int i = 0; i < text.Length; i++)
		{
			var ch = text[i];
			var idx = (int)ch;
			result[i] = ch switch
			{
				>= 'A' and <= 'Z' => (char)(A + (25 - (idx - A))),
				>= 'a' and <= 'z' => (char)(a + (25 - (idx - a))),
				_ => ch,
			};
		}

		written = text.Length;
	}
}