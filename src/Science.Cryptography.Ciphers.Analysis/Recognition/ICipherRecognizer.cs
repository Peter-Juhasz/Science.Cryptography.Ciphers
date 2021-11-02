using System;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Represents a recognition algorithm for a specific cipher.
    /// </summary>
    public interface ICipherRecognizer
    {
        /// <summary>
        /// Tries to recognize a cipher from <paramref name="ciphertext"/>.
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        RecognitionResult Recognize(ReadOnlySpan<char> ciphertext);
    }
}
