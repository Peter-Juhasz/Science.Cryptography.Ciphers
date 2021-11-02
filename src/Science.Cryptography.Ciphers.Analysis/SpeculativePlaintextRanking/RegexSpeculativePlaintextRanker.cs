using System;
using System.Text.RegularExpressions;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
    /// </summary>
    public sealed class RegexSpeculativePlaintextRanker : ISpeculativePlaintextScorer
    {
        public RegexSpeculativePlaintextRanker(Regex regex)
        {
            _regex = regex;
        }

        private readonly Regex _regex;


        /// <summary>
        /// Return 1 when the substring is found in the candidate, 0 if not.
        /// </summary>
        /// <param name="speculativePlaintext"></param>
        /// <returns></returns>
        public double Score(ReadOnlySpan<char> speculativePlaintext)
        {
            return _regex.IsMatch(new string(speculativePlaintext)) ? 1 : 0;
        }
    }
}
