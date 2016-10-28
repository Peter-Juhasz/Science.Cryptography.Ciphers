using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Finds cipher based on ciphertext characteristics.
    /// </summary>
    public static class Recognizer
    {
        public static IReadOnlyCollection<RecognitionResult> Recognize(string ciphertext)
        {
            if (ciphertext == null)
                throw new ArgumentNullException(nameof(ciphertext));
            
            return (
                from type in typeof(ICipherRecognizer).GetTypeInfo().Assembly.ExportedTypes
                let typeInfo = type.GetTypeInfo()
                where !typeInfo.IsAbstract
                let recognizer = Activator.CreateInstance(type) as ICipherRecognizer
                let result = recognizer.Recognize(ciphertext)
                where result.Probability > 0
                orderby result.Probability descending
                select result
            ).ToList();
        }
    }
}
