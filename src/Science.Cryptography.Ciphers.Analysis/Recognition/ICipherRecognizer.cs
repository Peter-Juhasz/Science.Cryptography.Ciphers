using System;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Represents a recognition algorithm for a specific cipher.
/// </summary>
public interface ICipherRecognizer
{
	/// <summary>
	/// Tries to recognize a cipher from <paramref name="ciphertext"/>.
	/// </summary>
	/// <param name="ciphertext"></param>
	/// <returns></returns>
	RecognitionResult? Recognize(ReadOnlySpan<char> ciphertext);
}

public sealed class MorseCodeRecognizer : ICipherRecognizer
{
	public RecognitionResult? Recognize(ReadOnlySpan<char> ciphertext)
	{
		int metaCount = 0;
		int whiteSpaceCount = 0;
		int otherCount = 0;

		foreach (var ch in ciphertext)
		{
			if (ch is '-' or '.') metaCount++;
			else if (Char.IsWhiteSpace(ch)) whiteSpaceCount++;
			else otherCount++;
		}

		return null;
	}
}