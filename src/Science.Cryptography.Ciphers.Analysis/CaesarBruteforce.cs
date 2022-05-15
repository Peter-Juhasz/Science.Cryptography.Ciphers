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

		for (int i = 0; i < alphabet.Length; i++)
		{
			dictionary[i] = cipher.Encrypt(text, i);
		}

		return dictionary;
	}
}
