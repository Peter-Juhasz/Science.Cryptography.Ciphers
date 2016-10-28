using System;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Represents the result of a cipher recognition.
    /// </summary>
    public class RecognitionResult
    {
        public RecognitionResult(string cipherId, double probability)
        {
            if (cipherId == null)
                throw new ArgumentNullException(nameof(cipherId));

            if (probability < 0 || probability > 1)
                throw new ArgumentOutOfRangeException(nameof(probability));

            this.CipherId = cipherId;
            this.Probability = probability;
        }
        
        /// <summary>
        /// Gets the identified cipher.
        /// </summary>
        public string CipherId { get; private set; }

        /// <summary>
        /// Gets the probability of the successful recognition.
        /// </summary>
        public double Probability { get; private set; }
    }
}
