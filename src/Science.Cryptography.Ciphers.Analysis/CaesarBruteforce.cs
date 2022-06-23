using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Analysis method for breaking Shift ciphers.
/// </summary>
public static class CaesarBruteforce
{
	public static IReadOnlyDictionary<int, string> Analyze(string text, Alphabet alphabet)
	{
		var cipher = new ShiftCipher(alphabet);
		var dictionary = new Dictionary<int, string>(capacity: alphabet.Length);
		Analyze(text, cipher, dictionary);
		return dictionary;
	}


	internal static void Analyze(string text, ShiftCipher cipher, IDictionary<int, string> output)
	{
		var length = cipher.Alphabet.Length;
		for (int i = 0; i < length; i++)
		{
			output[i] = cipher.Encrypt(text, i);
		}
	}

	internal static void Analyze(string text, ShiftCipher cipher, Span<string> output)
	{
		var length = cipher.Alphabet.Length;
		if (output.Length < length)
		{
			throw new ArgumentException("The size of the output buffer is not sufficient.", nameof(output));
		}

		for (int i = 0; i < length; i++)
		{
			output[i] = cipher.Encrypt(text, i);
		}
	}
}
