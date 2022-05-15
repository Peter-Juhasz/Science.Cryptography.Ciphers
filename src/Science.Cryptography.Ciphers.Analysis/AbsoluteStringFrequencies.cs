using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis;

public record struct AbsoluteStringFrequencies(IReadOnlyDictionary<string, int> Frequencies) : IReadOnlyDictionary<string, int>
{
	/// <summary>
	/// Gets the occurrences of a given <paramref name="character"/>.
	/// </summary>
	/// <param name="character"></param>
	/// <returns></returns>
	public int this[string character]
	{
		get
		{
			Frequencies.TryGetValue(character, out int frequency);
			return frequency;
		}
	}


	public RelativeStringFrequencies ToRelativeFrequencies()
	{
		var sum = (double)Frequencies.Sum(f => f.Value);
		return new(
			Frequencies.ToDictionary(
				kv => kv.Key,
				kv => kv.Value / sum
			)
		);
	}


	public IReadOnlyDictionary<string, int> ToDictionary() => Frequencies;

	#region IReadOnlyDictionary<char, int>
	IEnumerable<string> IReadOnlyDictionary<string, int>.Keys => Frequencies.Keys;
	IEnumerable<int> IReadOnlyDictionary<string, int>.Values => Frequencies.Values;

	int IReadOnlyCollection<KeyValuePair<string, int>>.Count => Frequencies.Count;

	int IReadOnlyDictionary<string, int>.this[string key] => this[key];

	bool IReadOnlyDictionary<string, int>.ContainsKey(string key) => Frequencies.ContainsKey(key);

	bool IReadOnlyDictionary<string, int>.TryGetValue(string key, out int value) => Frequencies.TryGetValue(key, out value);

	IEnumerator<KeyValuePair<string, int>> IEnumerable<KeyValuePair<string, int>>.GetEnumerator() => Frequencies.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => Frequencies.GetEnumerator();
	#endregion
}
