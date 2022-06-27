using BenchmarkDotNet.Attributes;
using Science.Cryptography.Ciphers.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[MemoryDiagnoser]
public class V1V2NGramAnalysisBenchmarks
{
	//private static readonly ISpeculativePlaintextScorer General = new RelativeLetterFrequenciesSpeculativePlaintextScorer(Languages.English.RelativeFrequenciesOfLetters).GetForPartition();
	//private static readonly ISpeculativePlaintextScorer Ascii = new TwoGramAsciiLettersFrequenciesSpeculativePlaintextScorer(Languages.English.RelativeFrequenciesOfLetters).GetForPartition();

	private static readonly string Text = "the quick brown fox jumps over the lazy dog";
	private static readonly Dictionary<int, int> Buffer = new(capacity: 26 * 26);

	[Benchmark]
	public void V1()
	{
		foreach (var _ in Analyze(Text, 2)) ;
	}

	[Benchmark]
	public void V2()
	{
		foreach (var _ in NGramAnalysis.AnalyzeLetters(Text, 2)) ;
	}

	[Benchmark]
	public void V2_Ascii_2()
	{
		NGramAnalysis.AnalyzeAsciiLetterTwoGrams(Text, Buffer);
	}

	public static IEnumerable<string> ReadNGrams(string text, int n)
	{
		if (text == null)
			throw new ArgumentNullException(nameof(text));

		if (n <= 0)
			throw new ArgumentOutOfRangeException(nameof(n));

		StringBuilder window = new StringBuilder(n);

		for (int i = 0; i <= text.Length - n; i++)
		{
			window.Clear();

			for (int j = 0; j < n; j++)
				window.Append(text[i + j]);

			yield return window.ToString();
		}
	}

	public static IReadOnlyDictionary<string, int> Analyze(string text, int n)
	{
		return ReadNGrams(text, n)
			.GroupBy(s => s)
			.ToDictionary(g => g.Key, g => g.Count());
	}

}