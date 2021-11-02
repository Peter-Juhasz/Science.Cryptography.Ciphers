using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Contains n-gram analysis tools.
/// </summary>
public static class NGramAnalysis
{
    /// <summary>
    /// Converts a given <paramref name="text"/> to <paramref name="length"/>-length sequences (including overlapping ones).
    /// </summary>
    /// <param name="text">The text to convert.</param>
    /// <param name="length">The length of the sequences.</param>
    /// <returns>All <paramref name="length"/>-grams.</returns>
    /// <example>ABCD to 2-grams: AB, BC, CD</example>
    public static IEnumerable<StringSegment> ReadNGrams(this string text, int length)
    {
        if (length <= 0)
            throw new ArgumentOutOfRangeException(nameof(length));

        for (int i = 0; i <= text.Length - length; i++)
        {
            yield return new StringSegment(text, i, length);
        }
    }

    /// <summary>
    /// Measures <paramref name="n"/>-gram frequencies in a given text.
    /// </summary>
    /// <param name="text">Text to analyze.</param>
    /// <param name="n">The length of n-gram sequences.</param>
    /// <returns>Frequencies of <paramref name="n"/>-grams in the text.</returns>
    public static IReadOnlyDictionary<string, int> Analyze(string text, int n)
    {
        return text.ReadNGrams(n)
            .GroupBy(s => s)
            .ToDictionary(g => g.Key.Value, g => g.Count());
    }
}
