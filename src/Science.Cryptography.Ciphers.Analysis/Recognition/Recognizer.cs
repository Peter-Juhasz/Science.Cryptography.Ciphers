using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Finds cipher based on ciphertext characteristics.
/// </summary>
public static class Recognizer
{
    public static RecognitionResult Recognize(
        ReadOnlySpan<char> ciphertext,
        Action<RecognitionResult>? onBetterResultFound = null,
        CancellationToken cancellationToken = default
    )
    {
        RecognitionResult best = null;
        object syncRoot = new();
        var boxed = new string(ciphertext);

        Parallel.ForEach(
            GetRecognizers(),
            new ParallelOptions { CancellationToken = cancellationToken },
            recognizer =>
            {
                RecognitionResult result = recognizer.Recognize(boxed);

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
