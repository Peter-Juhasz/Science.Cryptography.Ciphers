using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Analysis method for breaking Shift ciphers.
/// </summary>
public static class CaesarBruteforce
{
    public static IReadOnlyDictionary<int, string> Analyze(string text, Alphabet alphabet)
    {
        var cipher = new ShiftCipher(alphabet);

        return Enumerable.Range(0, alphabet.Length)
            .ToDictionary(k => k, k => cipher.Encrypt(text, k))
        ;
    }
}
