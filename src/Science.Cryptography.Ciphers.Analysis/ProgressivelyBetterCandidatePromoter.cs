namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Promotes progressively better candidates only.
/// </summary>
public sealed class ProgressivelyBetterCandidatePromoter : ICandidatePromoter
{
	public ProgressivelyBetterCandidatePromoter(double initialScore)
	{
		BestScore = initialScore;
	}
	public ProgressivelyBetterCandidatePromoter()
		: this(0D)
	{ }

	public double BestScore { get; private set; }

	public bool Promote(double score)
	{
		if (score > BestScore)
		{
			BestScore = score;
			return true;
		}

		return false;
	}
}
