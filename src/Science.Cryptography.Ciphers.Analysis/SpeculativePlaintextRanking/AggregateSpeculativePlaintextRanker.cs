using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
    /// </summary>
    public sealed class AggregateSpeculativePlaintextRanker : ISpeculativePlaintextRanker
    {
        public AggregateSpeculativePlaintextRanker(
            IReadOnlyCollection<ISpeculativePlaintextRanker> rankers,
            Func<double, double, double> accumulator
        )
        {
            if (rankers == null)
                throw new ArgumentNullException(nameof(rankers));

            if (accumulator == null)
                throw new ArgumentNullException(nameof(accumulator));

            _rankers = rankers;
            _accumulator = accumulator;
        }

        private readonly IReadOnlyCollection<ISpeculativePlaintextRanker> _rankers;
        private readonly Func<double, double, double> _accumulator;
        

        /// <summary>
        /// Return 1 when the substring is found in the candidate, 0 if not.
        /// </summary>
        /// <param name="speculativePlaintext"></param>
        /// <returns></returns>
        public double Classify(string speculativePlaintext)
        {
            return _rankers.Select(r => r.Classify(speculativePlaintext)).Aggregate(_accumulator);
        }
    }
}
