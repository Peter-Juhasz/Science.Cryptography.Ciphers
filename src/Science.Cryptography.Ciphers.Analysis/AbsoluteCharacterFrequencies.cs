using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis;

public record class AbsoluteCharacterFrequencies(IReadOnlyDictionary<char, int> Frequencies) : IReadOnlyDictionary<char, int>
{
    /// <summary>
    /// Gets the occurrences of a given <paramref name="character"/>.
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public int this[char character]
    {
        get
        {
            Frequencies.TryGetValue(character, out int frequency);
            return frequency;
        }
    }


    public RelativeCharacterFrequencies ToRelativeFrequencies()
    {
        var sum = (double)Frequencies.Sum(f => f.Value);
        return new(
            Frequencies.ToDictionary(
                kv => kv.Key,
                kv => kv.Value / sum
            )
        );
    }


    public IReadOnlyDictionary<char, int> ToDictionary() => Frequencies;

    #region IReadOnlyDictionary<char, int>
    IEnumerable<char> IReadOnlyDictionary<char, int>.Keys => Frequencies.Keys;
    IEnumerable<int> IReadOnlyDictionary<char, int>.Values => Frequencies.Values;

    int IReadOnlyCollection<KeyValuePair<char, int>>.Count => Frequencies.Count;

    int IReadOnlyDictionary<char, int>.this[char key] => this[key];

    bool IReadOnlyDictionary<char, int>.ContainsKey(char key) => Frequencies.ContainsKey(key);

    bool IReadOnlyDictionary<char, int>.TryGetValue(char key, out int value) => Frequencies.TryGetValue(key, out value);

    IEnumerator<KeyValuePair<char, int>> IEnumerable<KeyValuePair<char, int>>.GetEnumerator() => Frequencies.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Frequencies.GetEnumerator();
    #endregion
}
