using System;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Represents the result of a cipher recognition.
    /// </summary>
    public class RecognitionResult
    {
        public RecognitionResult(string cipherId, double probability)
            : this(succeeded: true)
        {
            if (cipherId == null)
                throw new ArgumentNullException(nameof(cipherId));

            if (probability < 0 || probability > 1)
                throw new ArgumentOutOfRangeException(nameof(probability));

            this.CipherId = cipherId;
            this.Probability = probability;
        }
        private RecognitionResult(bool succeeded)
        {
            this.Succeeded = succeeded;
        }

        public static readonly RecognitionResult Failed = new RecognitionResult(succeeded: false);


        /// <summary>
        /// Gets whether the recognition succeeded or not.
        /// </summary>
        public bool Succeeded { get; private set; }

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
