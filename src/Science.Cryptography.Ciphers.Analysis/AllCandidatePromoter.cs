namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Promotes all candidates as final results.
/// </summary>
public sealed class AllCandidatePromoter : ICandidatePromoter
{
	public static readonly ICandidatePromoter Instance = new AllCandidatePromoter();

	public bool Promote(double score) => true;
}
