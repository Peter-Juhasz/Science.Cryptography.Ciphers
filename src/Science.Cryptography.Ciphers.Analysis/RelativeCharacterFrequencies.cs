using System;
using System.Collections;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// 
    /// </summary>
    public class RelativeCharacterFrequencies : IReadOnlyDictionary<char, double>
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
