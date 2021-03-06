﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
    /// </summary>
    public sealed class WordlistSpeculativePlaintextRanker : ISpeculativePlaintextRanker
    {
        public WordlistSpeculativePlaintextRanker(IReadOnlyCollection<string> wordlist, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (wordlist == null)
                throw new ArgumentNullException(nameof(wordlist));

            _orderedWordlist = wordlist.OrderByDescending(w => w.Length).ToList();
            _comparison = comparison;
        }

        private readonly IReadOnlyList<string> _orderedWordlist;
        private readonly StringComparison _comparison;


        /// <summary>
        /// Return 1 when the substring is found in the candidate, 0 if not.
        /// </summary>
        /// <param name="speculativePlaintext"></param>
        /// <returns></returns>
        public double Classify(string speculativePlaintext)
        {
            return ((double)speculativePlaintext.Length - _orderedWordlist.Aggregate(speculativePlaintext, (r, n) => r.Replace(n, String.Empty, _comparison)).Length) / speculativePlaintext.Length;
        }
    }
}
