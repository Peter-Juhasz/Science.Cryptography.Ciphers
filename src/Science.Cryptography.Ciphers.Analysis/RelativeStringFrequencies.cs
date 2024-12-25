using System.Collections;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

public readonly record struct RelativeStringFrequencies(IReadOnlyDictionary<string, double> Frequencies) : IReadOnlyDictionary<string, double>
{
	/// <summary>
	/// Gets the occurrences of a given <paramref name="character"/>.
	/// </summary>
	/// <param name="character"></param>
	/// <returns></returns>
	public readonly double this[string character]
	{
		get
		{
			Frequencies.TryGetValue(character, out double frequency);
			return frequency;
		}
	}

	public readonly bool TryGetValue(string key, out double value) => Frequencies.TryGetValue(key, out value);

	public readonly IReadOnlyDictionary<string, double> ToDictionary() => Frequencies;


	#region IReadOnlyDictionary<char, double>
	IEnumerable<string> IReadOnlyDictionary<string, double>.Keys => Frequencies.Keys;
	IEnumerable<double> IReadOnlyDictionary<string, double>.Values => Frequencies.Values;

	int IReadOnlyCollection<KeyValuePair<string, double>>.Count => Frequencies.Count;

	double IReadOnlyDictionary<string, double>.this[string key] => this[key];

	bool IReadOnlyDictionary<string, double>.ContainsKey(string key) => Frequencies.ContainsKey(key);

	bool IReadOnlyDictionary<string, double>.TryGetValue(string key, out double value) => Frequencies.TryGetValue(key, out value);

	IEnumerator<KeyValuePair<string, double>> IEnumerable<KeyValuePair<string, double>>.GetEnumerator() => Frequencies.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => Frequencies.GetEnumerator();
	#endregion
}
