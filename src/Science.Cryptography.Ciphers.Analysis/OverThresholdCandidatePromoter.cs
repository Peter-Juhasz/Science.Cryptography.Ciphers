namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Promotes all candidates above a specified threshold.
/// </summary>
public sealed class OverThresholdCandidatePromoter : ICandidatePromoter
{
	public OverThresholdCandidatePromoter(double scoreThreshold)
	{
		Threshold = scoreThreshold;
	}

	public double Threshold { get; }

	public bool Promote(double score) => score >= Threshold;
}
