using System;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
    /// </summary>
    public sealed class SubstringSpeculativePlaintextRanker : ISpeculativePlaintextRanker
    {
        public SubstringSpeculativePlaintextRanker(string substring, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (substring == null)
                throw new ArgumentNullException(nameof(substring));

            _substring = substring;
            _comparison = comparison;
        }

        private readonly string _substring;
        private readonly StringComparison _comparison;


        /// <summary>
        /// Return 1 when the substring is found in the candidate, 0 if not.
        /// </summary>
        /// <param name="speculativePlaintext"></param>
        /// <returns></returns>
        public double Classify(string speculativePlaintext)
        {
            return speculativePlaintext.IndexOf(_substring, _comparison) != -1 ? 1 : 0;
        }
    }
}
