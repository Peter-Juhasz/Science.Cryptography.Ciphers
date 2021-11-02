using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers;

public static partial class StringExtensions
{
    public static int IndexOfIgnoreCase(this string source, char subject)
    {
        Char toCompare = subject.ToUpper();

        for (int i = 0; i < source.Length; i++)
        {
            if (source[i].ToUpper() == toCompare)
                return i;
        }

        return -1;
    }

    public static char At(this string source, int index)
    {
        return source[index.Mod(source.Length)];
    }

    internal static IEnumerable<string> Split(this string source, int chunkSize)
    {
        int offset = 0;

        while (offset <= source.Length - chunkSize)
        {
            yield return source.Substring(offset, chunkSize);

            offset += chunkSize;
        }
    }

    internal static string Capitalize(this string text)
    {
        return text.First().ToUpper() + text.Substring(1);
    }

    public static string EfficientSelect(this string text, Func<char, char> selector)
    {
        char[] ciphertext = new char[text.Length];

        for (int i = 0; i < text.Length; i++)
            ciphertext[i] = selector(text[i]);

        return new String(ciphertext);
    }
}
