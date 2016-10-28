namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Classifies potential plaintext candidates on how likely are they meaningful results.
    /// </summary>
    public interface IMeaningfulnessClassifier
    {
        double Classify(string text);
    }
}
