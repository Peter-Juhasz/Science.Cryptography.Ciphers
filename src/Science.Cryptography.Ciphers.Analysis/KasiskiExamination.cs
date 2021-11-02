using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Science.Cryptography.Ciphers.Analysis
{
    public static class KasiskiExamination
    {
        public static KasiskiExaminationResult Analyze(
            string ciphertext,
            int minimumKeyLength = 3,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            if (ciphertext == null)
                throw new ArgumentNullException(nameof(ciphertext));

            if (minimumKeyLength < 0 || minimumKeyLength > ciphertext.Length)
                throw new ArgumentOutOfRangeException(nameof(minimumKeyLength));


            List<int> speculativeKeyLengths = new List<int>();
            object syncRoot = new object();

            Parallel.For(
                minimumKeyLength, ciphertext.Length / 2,
                new ParallelOptions { CancellationToken = cancellationToken },
                keylength =>
                {
                    for (int i = 0; i < ciphertext.Length - 2 * keylength; i++)
                    {
                        string substring = ciphertext.Substring(i, keylength);

                        foreach (int nextOccurrence in
                            from o in ciphertext.AllIndexesOf(substring, i + 1, comparison)
                            where (o - i) % keylength == 0
                            select o
                        )
                        {
                            int length = nextOccurrence - i;

                            lock (syncRoot)
                            {
                                if (speculativeKeyLengths.Any(l => keylength % l == 0))
                                    continue;

                                if (speculativeKeyLengths.Any(l => l % keylength == 0))
                                    speculativeKeyLengths.RemoveAll(l => l % keylength == 0);

                                speculativeKeyLengths.Add(keylength);
                            }
                        }
                    }
                }
            );

            return new KasiskiExaminationResult(speculativeKeyLengths);
        }


        private static IReadOnlyCollection<int> Divisors(int n)
        {
            return (
                from d in Enumerable.Range(1, n)
                where n % d == 0
                select d
            ).ToList();
        }
    }
}
