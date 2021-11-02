using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
/// </summary>
public sealed class CharsetSpeculativePlaintextRanker : ISpeculativePlaintextScorer
{
    public CharsetSpeculativePlaintextRanker(IReadOnlySet<char> charset)
    {
        Charset = charset;
    }

    public IReadOnlySet<char> Charset { get; }


    /// <summary>
    /// Return 1 when the substring is found in the candidate, 0 if not.
    /// </summary>
    /// <param name="speculativePlaintext"></param>
    /// <returns></returns>
    public double Score(ReadOnlySpan<char> speculativePlaintext)
    {
        var count = 0;
        foreach (var ch in speculativePlaintext) if (Charset.Contains(ch)) count++;

        return (double)count / speculativePlaintext.Length;
    }
}
