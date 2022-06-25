using BenchmarkDotNet.Attributes;

using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using Science.Cryptography.Ciphers.Analysis;

[MemoryDiagnoser]
public class V1V2RelativeLetterFrequenciesScorerBenchmarks
{
	private static readonly ISpeculativePlaintextScorer General = new RelativeLetterFrequenciesSpeculativePlaintextScorer(Languages.English.RelativeFrequenciesOfLetters).GetForPartition();
	private static readonly ISpeculativePlaintextScorer Ascii = new AsciiRelativeLetterFrequenciesSpeculativePlaintextScorer(Languages.English.RelativeFrequenciesOfLetters).GetForPartition();

	private static readonly string Text = "the quick brown fox jumps over the lazy dog";


	[Benchmark]
	public void V1()
	{
		Classify(Text);
	}

	[Benchmark]
	public void V2()
	{
		General.Score(Text);
	}

	[Benchmark]
	public void V2_Ascii()
	{
		Ascii.Score(Text);
	}

	public double Classify(string speculativePlaintext)
	{
		return Compare(Languages.English.RelativeFrequenciesOfLetters, Analyze(speculativePlaintext).AsRelativeFrequencies());
	}

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

	private static double Compare(IReadOnlyDictionary<char, double> reference, IReadOnlyDictionary<char, double> subject)
	{
		return 1 - (
			from r in reference
			select Math.Abs(r.Value - (subject.ContainsKey(r.Key) ? subject[r.Key] : 0))
		).Sum();
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
	private class RelativeCharacterFrequencies : IReadOnlyDictionary<char, double>
	{
		public RelativeCharacterFrequencies(IReadOnlyDictionary<char, double> frequencies)
		{
			if (frequencies == null)
				throw new ArgumentNullException(nameof(frequencies));

			_frequencies = frequencies;
		}

		private readonly IReadOnlyDictionary<char, double> _frequencies;

		/// <summary>
		/// Gets the occurrences of a given <paramref name="character"/>.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public double this[char character]
		{
			get
			{
				double frequency = 0;
				_frequencies.TryGetValue(character, out frequency);
				return frequency;
			}
		}


		public IReadOnlyDictionary<char, double> ToDictionary() => _frequencies;


		#region IReadOnlyDictionary<char, double>
		IEnumerable<char> IReadOnlyDictionary<char, double>.Keys => _frequencies.Keys;
		IEnumerable<double> IReadOnlyDictionary<char, double>.Values => _frequencies.Values;

		int IReadOnlyCollection<KeyValuePair<char, double>>.Count => _frequencies.Count;

		double IReadOnlyDictionary<char, double>.this[char key] => this[key];

		bool IReadOnlyDictionary<char, double>.ContainsKey(char key) => _frequencies.ContainsKey(key);

		bool IReadOnlyDictionary<char, double>.TryGetValue(char key, out double value) => _frequencies.TryGetValue(key, out value);

		IEnumerator<KeyValuePair<char, double>> IEnumerable<KeyValuePair<char, double>>.GetEnumerator() => _frequencies.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _frequencies.GetEnumerator();
		#endregion
	}
}