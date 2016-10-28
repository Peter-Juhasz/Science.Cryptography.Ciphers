using System;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Searches for a specific known portion of plaintext to classify potential plaintext candidates.
    /// </summary>
    public sealed class SubstringMeaningfulnessClassifier : IMeaningfulnessClassifier
    {
        public SubstringMeaningfulnessClassifier(string substring)
        {
            if (substring == null)
                throw new ArgumentNullException();

            _substring = substring;
        }

        private readonly string _substring;


        /// <summary>
        /// Return 1 when the substring is found in the candidate, 0 if not.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public double Classify(string candidate)
        {
            return candidate.Contains(_substring) ? 1 : 0;
        }
    }
}
