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
        public static double Analyze(ReadOnlySpan<char> input, double logarithmBase = 2)
        {
            var length = input.Length;
            return -(
                from kv in FrequencyAnalysis.Analyze(input)
                let occurrence = kv.Value
                let probability = occurrence / length
                select probability * Math.Log(probability, logarithmBase)
            ).Sum();
        }
    }
}
