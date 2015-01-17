namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Amplifies ciphers with the ability to recognize ciphertexts.
    /// </summary>
    public interface ISupportsRecognition
    {
        /// <summary>
        /// Determines whether the <paramref name="ciphertext" /> was encrypted with the given cipher or not.
        /// </summary>
        /// <param name="ciphertext">The encrypted text to analyze.</param>
        /// <returns></returns>
        bool Recognize(string ciphertext);
    }
}
