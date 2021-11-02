using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Measures character frequencies.
/// </summary>
public static class FrequencyAnalysis
{
    public static AbsoluteCharacterFrequencies Analyze(ReadOnlySpan<char> text)
    {
        var result = new Dictionary<char, int>();
        for (int i = 0; i < text.Length; i++)
        {
            var ch = text[i];
            ref int frequency = ref CollectionsMarshal.GetValueRefOrAddDefault(result, ch, out _);
            frequency++;
        }

        return new(result);
    }

    public static AbsoluteCharacterFrequencies AnalyzeIgnoreCase(ReadOnlySpan<char> text)
    {
        var result = new Dictionary<char, int>(IgnoreCaseCharComparer.Instance);
        for (int i = 0; i < text.Length; i++)
        {
            var ch = text[i];
            ref int frequency = ref CollectionsMarshal.GetValueRefOrAddDefault(result, ch, out _);
            frequency++;
        }

        return new(result);
    }

    public static double Compare(IReadOnlyDictionary<char, double> reference, IReadOnlyDictionary<char, double> subject)
    {
        return 1 - (
            from r in reference
            select Math.Abs(r.Value - (subject.TryGetValue(r.Key, out var v) ? v : 0))
        ).Sum();
    }
}
