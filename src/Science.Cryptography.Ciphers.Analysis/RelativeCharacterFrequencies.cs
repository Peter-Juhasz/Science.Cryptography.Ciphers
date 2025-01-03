using System.Collections;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

public readonly record struct RelativeCharacterFrequencies(IReadOnlyDictionary<char, double> Frequencies) : IReadOnlyDictionary<char, double>
{
	/// <summary>
	/// Gets the occurrences of a given <paramref name="character"/>.
	/// </summary>
	/// <param name="character"></param>
	/// <returns></returns>
	public readonly double this[char character]
	{
		get
		{
			Frequencies.TryGetValue(character, out double frequency);
			return frequency;
		}
	}

	public readonly bool TryGetValue(char key, out double value) => Frequencies.TryGetValue(key, out value);

	public readonly IReadOnlyDictionary<char, double> ToDictionary() => Frequencies;


	#region IReadOnlyDictionary<char, double>
	IEnumerable<char> IReadOnlyDictionary<char, double>.Keys => Frequencies.Keys;
	IEnumerable<double> IReadOnlyDictionary<char, double>.Values => Frequencies.Values;

	int IReadOnlyCollection<KeyValuePair<char, double>>.Count => Frequencies.Count;

	double IReadOnlyDictionary<char, double>.this[char key] => this[key];

	bool IReadOnlyDictionary<char, double>.ContainsKey(char key) => Frequencies.ContainsKey(key);

	bool IReadOnlyDictionary<char, double>.TryGetValue(char key, out double value) => Frequencies.TryGetValue(key, out value);

	IEnumerator<KeyValuePair<char, double>> IEnumerable<KeyValuePair<char, double>>.GetEnumerator() => Frequencies.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => Frequencies.GetEnumerator();
	#endregion
}
