using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
    /// </summary>
    public sealed class RelativeLetterFrequenciesSpeculativePlaintextRanker : ISpeculativePlaintextRanker
    {
        public RelativeLetterFrequenciesSpeculativePlaintextRanker(IReadOnlyDictionary<char, double> relativeFrequencies)
        {
            if (relativeFrequencies == null)
                throw new ArgumentNullException(nameof(relativeFrequencies));

            _relativeFrequencies = relativeFrequencies;
        }
        public RelativeLetterFrequenciesSpeculativePlaintextRanker(ILanguageStatisticalInfo language)
            : this(language.RelativeFrequenciesOfLetters)
        { }

        private readonly IReadOnlyDictionary<char, double> _relativeFrequencies;


        /// <summary>
        /// Return 1 when the substring is found in the candidate, 0 if not.
        /// </summary>
        /// <param name="speculativePlaintext"></param>
        /// <returns></returns>
        public double Classify(string speculativePlaintext)
        {
            return FrequencyAnalysis.Compare(_relativeFrequencies, FrequencyAnalysis.Analyze(speculativePlaintext).AsRelativeFrequencies());
        }
    }
}
