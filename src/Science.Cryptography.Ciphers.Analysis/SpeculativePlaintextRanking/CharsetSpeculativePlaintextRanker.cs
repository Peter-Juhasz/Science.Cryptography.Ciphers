using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
    /// </summary>
    public sealed class CharsetSpeculativePlaintextRanker : ISpeculativePlaintextRanker
    {
        public CharsetSpeculativePlaintextRanker(IReadOnlyCollection<char> charset)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            _charset = charset;
        }

        private readonly IReadOnlyCollection<char> _charset;


        /// <summary>
        /// Return 1 when the substring is found in the candidate, 0 if not.
        /// </summary>
        /// <param name="speculativePlaintext"></param>
        /// <returns></returns>
        public double Classify(string speculativePlaintext)
        {
            return (double)speculativePlaintext.Count(_charset.Contains) / speculativePlaintext.Length;
        }
    }
}
