using System;
using System.Threading;
using System.Threading.Tasks;

namespace Science.Cryptography.Ciphers.Analysis
{
    public static class KeyFinder
    {
        public static KeyFinderResult FindBest<T>(
            string ciphertext,
            IKeyedCipher<T> cipher,
            IKeySpaceSource<T> keySpace,
            ISpeculativePlaintextRanker speculativePlaintextRanker,
            Action<KeyFinderResult> onBetterResultFound = null,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            if (ciphertext == null)
                throw new ArgumentNullException(nameof(ciphertext));

            if (cipher == null)
                throw new ArgumentNullException(nameof(cipher));

            if (keySpace == null)
                throw new ArgumentNullException(nameof(keySpace));

            if (speculativePlaintextRanker == null)
                throw new ArgumentNullException(nameof(speculativePlaintextRanker));


            KeyFinderResult best = null;
            object syncRoot = new object();

            Parallel.ForEach(
                keySpace.GetKeys(),
                new ParallelOptions { CancellationToken = cancellationToken },
                key =>
                {
                    string speculativePlaintext = cipher.Decrypt(ciphertext, key);
                    double rank = speculativePlaintextRanker.Classify(speculativePlaintext);

                    if (rank > (best?.Rank ?? 0))
                    {
                        lock (syncRoot)
                        {
                            if (rank > (best?.Rank ?? 0))
                            {
                                best = new KeyFinderResult(speculativePlaintext, rank);

                                onBetterResultFound?.Invoke(best);
                            }
                        }
                    }
                }
            );

            return best;
        }

        public static void FindAboveThreshold<T>(
            string ciphertext,
            IKeyedCipher<T> cipher,
            IKeySpaceSource<T> keySpace,
            ISpeculativePlaintextRanker speculativePlaintextRanker,
            Action<KeyFinderResult> onImmediateResultFound,
            double threshold = 0.9,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            if (ciphertext == null)
                throw new ArgumentNullException(nameof(ciphertext));

            if (cipher == null)
                throw new ArgumentNullException(nameof(cipher));

            if (keySpace == null)
                throw new ArgumentNullException(nameof(keySpace));

            if (speculativePlaintextRanker == null)
                throw new ArgumentNullException(nameof(speculativePlaintextRanker));

            if (threshold < 0 || threshold > 1)
                throw new ArgumentOutOfRangeException(nameof(threshold));

            if (onImmediateResultFound == null)
                throw new ArgumentNullException(nameof(onImmediateResultFound));


            Parallel.ForEach(
                keySpace.GetKeys(),
                new ParallelOptions { CancellationToken = cancellationToken },
                key =>
                {
                    string speculativePlaintext = cipher.Decrypt(ciphertext, key);
                    double rank = speculativePlaintextRanker.Classify(speculativePlaintext);

                    if (rank >= threshold)
                        onImmediateResultFound(new KeyFinderResult(speculativePlaintext, rank));
                }
            );
        }
    }
}
