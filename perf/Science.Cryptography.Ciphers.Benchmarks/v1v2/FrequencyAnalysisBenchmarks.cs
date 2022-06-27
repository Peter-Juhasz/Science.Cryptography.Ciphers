using BenchmarkDotNet.Attributes;

using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using Science.Cryptography.Ciphers.Analysis;

[MemoryDiagnoser]
public class V1V2FrequencyAnalysisBenchmarks
{
	private static readonly Dictionary<char, int> _buffer = new Dictionary<char, int>();

	private static readonly string Text = "the quick brown fox jumps over the lazy dog";


	[Benchmark]
	public void V1()
	{
		Analyze(Text);
	}

	[Benchmark]
	public void V2()
	{
		FrequencyAnalysis.Analyze(Text);
	}

	[Benchmark]
	public void V2_Ascii()
	{
		FrequencyAnalysis.AnalyzeAsciiLetters(Text, _buffer);
	}

	#region V1
	private static AbsoluteCharacterFrequencies Analyze(string text)
	{
		if (text == null)
			throw new ArgumentNullException(nameof(text));

		return new AbsoluteCharacterFrequencies(
			text
				.GroupBy(c => c)
				.ToDictionary(g => g.Key, g => g.Count())
		);
	}

	private class AbsoluteCharacterFrequencies : IReadOnlyDictionary<char, int>
	{
		public AbsoluteCharacterFrequencies(IReadOnlyDictionary<char, int> frequencies)
		{
			if (frequencies == null)
				throw new ArgumentNullException(nameof(frequencies));

			_frequencies = frequencies;
		}

		private readonly IReadOnlyDictionary<char, int> _frequencies;

		/// <summary>
		/// Gets the occurrences of a given <paramref name="character"/>.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public int this[char character]
		{
			get
			{
				int frequency = 0;
				_frequencies.TryGetValue(character, out frequency);
				return frequency;
			}
		}


		public IReadOnlyDictionary<char, double> AsRelativeFrequencies()
		{
			var sum = (double)_frequencies.Sum(f => f.Value);
			return new RelativeCharacterFrequencies(
				_frequencies.ToDictionary(
					kv => kv.Key,
					kv => kv.Value / sum
				)
			);
		}


		public IReadOnlyDictionary<char, int> ToDictionary() => _frequencies;

		#region IReadOnlyDictionary<char, int>
		IEnumerable<char> IReadOnlyDictionary<char, int>.Keys => _frequencies.Keys;
		IEnumerable<int> IReadOnlyDictionary<char, int>.Values => _frequencies.Values;

		int IReadOnlyCollection<KeyValuePair<char, int>>.Count => _frequencies.Count;

		int IReadOnlyDictionary<char, int>.this[char key] => this[key];

		bool IReadOnlyDictionary<char, int>.ContainsKey(char key) => _frequencies.ContainsKey(key);

		bool IReadOnlyDictionary<char, int>.TryGetValue(char key, out int value) => _frequencies.TryGetValue(key, out value);

		IEnumerator<KeyValuePair<char, int>> IEnumerable<KeyValuePair<char, int>>.GetEnumerator() => _frequencies.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _frequencies.GetEnumerator();
		#endregion
	}
	#endregion
}