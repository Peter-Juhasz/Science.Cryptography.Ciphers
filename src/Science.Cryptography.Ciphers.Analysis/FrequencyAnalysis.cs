using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Measures character frequencies.
/// </summary>
public static class FrequencyAnalysis
{
	public static AbsoluteCharacterFrequencies AnalyzeLetters(ReadOnlySpan<char> text)
	{
		var result = new Dictionary<char, int>(capacity: 26, IgnoreCaseCharComparer.Instance);
		foreach (var ch in text)
		{
			if (Char.IsLetter(ch))
			{
				ref int frequency = ref CollectionsMarshal.GetValueRefOrAddDefault(result, ch, out _);
				frequency++;
			}
		}
		return new(result.ToFrozenDictionary());
	}

	internal static void AnalyzeLetters(ReadOnlySpan<char> text, Dictionary<char, int> output)
	{
		foreach (var ch in text)
		{
			if (Char.IsLetter(ch))
			{
				ref int frequency = ref CollectionsMarshal.GetValueRefOrAddDefault(output, ch, out _);
				frequency++;
			}
		}
	}

	internal static void AnalyzeAsciiLetters(ReadOnlySpan<char> text, Dictionary<char, int> output)
	{
		foreach (var ch in text)
		{
			if (ch.IsAsciiLetter())
			{
				ref int frequency = ref CollectionsMarshal.GetValueRefOrAddDefault(output, ch, out _);
				frequency++;
			}
		}
	}

	public static AbsoluteCharacterFrequencies AnalyzeAsciiLetters(ReadOnlySpan<char> text)
	{
		var dictionary = new Dictionary<char, int>(capacity: 26, IgnoreCaseCharComparer.Instance);
		AnalyzeAsciiLetters(text, dictionary);
		return new(dictionary.ToFrozenDictionary());
	}

	public static AbsoluteCharacterFrequencies Analyze(ReadOnlySpan<char> text, Predicate<char> filter, IEqualityComparer<char> comparer)
	{
		var result = new Dictionary<char, int>(comparer);
		foreach (var ch in text)
        {
            if (filter(ch))
            {
                ref int frequency = ref CollectionsMarshal.GetValueRefOrAddDefault(result, ch, out _);
                frequency++;
            }
        }
		return new(result.ToFrozenDictionary());
	}

	public static AbsoluteCharacterFrequencies Analyze(ReadOnlySpan<char> text) =>
		Analyze(text, _ => true, EqualityComparer<char>.Default);

	public static AbsoluteCharacterFrequencies Analyze(ReadOnlySpan<char> text, Alphabet alphabet) =>
		Analyze(text, c => alphabet.Contains(c, StringComparison.OrdinalIgnoreCase), IgnoreCaseCharComparer.Instance);

	public static double Compare(RelativeCharacterFrequencies reference, RelativeCharacterFrequencies subject) =>
		CompareCore(reference, subject);

	public static double Compare(RelativeCharacterFrequencies reference, AbsoluteCharacterFrequencies subject) =>
		CompareCore(reference, subject);

	public static double Compare(RelativeStringFrequencies reference, RelativeStringFrequencies subject) =>
		CompareCore(reference, subject);

	public static double Compare(RelativeStringFrequencies reference, AbsoluteStringFrequencies subject) =>
		CompareCore(reference, subject);

	internal static double CompareCore<T>(IReadOnlyDictionary<T, double> reference, IReadOnlyDictionary<T, double> actual)
	{
		if (actual.Count == 0)
		{
			return 0D;
		}

		int count = reference.Count;
		double diffs = 0D;

		foreach (var (key, referenceValue) in reference)
		{
			if (actual.TryGetValue(key, out var actualValue))
			{
				diffs += Math.Abs(actualValue - referenceValue);
			}
			else
			{
				diffs += 1D;
			}
		}

		int nonMatchingCount = 0;
		foreach (var actualKey in actual.Keys)
		{
			if (!reference.ContainsKey(actualKey))
			{
				diffs += 1D;
				nonMatchingCount++;
			}
		}

		return 1D - diffs / (count + nonMatchingCount);
	}

	internal static double CompareCore<T>(IReadOnlyDictionary<T, double> reference, IReadOnlyDictionary<T, int> actual)
	{
		if (actual.Count == 0)
		{
			return 0D;
		}

		var sum = (double)actual.Values.Sum();

		int count = reference.Count;
		double diffs = 0D;

		foreach (var (key, referenceValue) in reference)
		{
			if (actual.TryGetValue(key, out var actualValue))
			{
				diffs += Math.Abs(actualValue / sum - referenceValue);
			}
			else
			{
				diffs += 1D;
			}
		}

		int nonMatchingCount = 0;
		foreach (var actualKey in actual.Keys)
		{
			if (!reference.ContainsKey(actualKey))
			{
				diffs += 1D;
				nonMatchingCount++;
			}
		}

		return 1D - diffs / (count + nonMatchingCount);
	}
}
