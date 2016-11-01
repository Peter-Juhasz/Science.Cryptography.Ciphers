using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Finds cipher based on ciphertext characteristics.
    /// </summary>
    public static class Recognizer
    {
        public static IReadOnlyCollection<RecognitionResult> Recognize(string ciphertext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (ciphertext == null)
                throw new ArgumentNullException(nameof(ciphertext));
            
            return (
                from recognizer in GetRecognizers()
                let result = recognizer.Recognize(ciphertext)
                where !cancellationToken.IsCancellationRequested
                where result.Probability > 0
                orderby result.Probability descending
                select result
            ).ToList();
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
