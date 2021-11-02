using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
    /// </summary>
    public sealed class AggregateSpeculativePlaintextScorer : ISpeculativePlaintextScorer
    {
        public AggregateSpeculativePlaintextScorer(
            IReadOnlyCollection<ISpeculativePlaintextScorer> rankers,
            Func<double, double, double> accumulator
        )
        {
            Rankers = rankers;
            _accumulator = accumulator;
        }

        public IReadOnlyCollection<ISpeculativePlaintextScorer> Rankers { get; }
        private readonly Func<double, double, double> _accumulator;


        /// <summary>
        /// Return 1 when the substring is found in the candidate, 0 if not.
        /// </summary>
        /// <param name="speculativePlaintext"></param>
        /// <returns></returns>
        public double Score(ReadOnlySpan<char> speculativePlaintext)
        {
            var boxed = new string(speculativePlaintext);
            return Rankers.Select(r => r.Score(boxed)).Aggregate(_accumulator);
        }
    }
}
