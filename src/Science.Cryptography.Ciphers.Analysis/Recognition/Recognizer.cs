using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Finds cipher based on ciphertext characteristics.
    /// </summary>
    public static class Recognizer
    {
        public static RecognitionResult Recognize(
            string ciphertext,
            Action<RecognitionResult> onBetterResultFound = null,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            if (ciphertext == null)
                throw new ArgumentNullException(nameof(ciphertext));

            RecognitionResult best = null;
            object syncRoot = new object();

            Parallel.ForEach(
                GetRecognizers(),
                new ParallelOptions { CancellationToken = cancellationToken },
                recognizer =>
                {
                    RecognitionResult result = recognizer.Recognize(ciphertext);

                    if (result.Probability == 0)
                        return;

                    if (result.Probability > (best?.Probability ?? 0))
                    {
                        lock (syncRoot)
                        {
                            if (result.Probability > (best?.Probability ?? 0))
                            {
                                best = result;
                                onBetterResultFound?.Invoke(best);
                            }
                        }
                    }
                }
            );

            return best;
        }


        private static IReadOnlyCollection<ICipherRecognizer> GetRecognizers()
        {
            return (
                from type in typeof(ICipherRecognizer).GetTypeInfo().Assembly.ExportedTypes
                let typeInfo = type.GetTypeInfo()
                where !typeInfo.IsAbstract
                select Activator.CreateInstance(type) as ICipherRecognizer
            ).ToList();
        }
    }
}
