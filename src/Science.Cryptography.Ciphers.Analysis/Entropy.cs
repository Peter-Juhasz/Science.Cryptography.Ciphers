using System;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis
{
    public static class Entropy
    {
        /// <summary>
        /// Calculates the entropy of <paramref name="input" />.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="logarithmBase"></param>
        /// <returns></returns>
        public static double Analyze(string input, double logarithmBase = 2)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            return -(
                from kv in FrequencyAnalysis.Analyze(input)
                let occurrence = kv.Value
                let probability = (double)occurrence / input.Length
                select probability * Math.Log(probability, logarithmBase)
            ).Sum();
        }
    }
}
