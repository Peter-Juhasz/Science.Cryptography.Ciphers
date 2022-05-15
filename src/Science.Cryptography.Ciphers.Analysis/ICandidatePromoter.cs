namespace Science.Cryptography.Ciphers.Analysis;

public interface ICandidatePromoter
{
	bool Promote(double score);

	bool Promote(SpeculativePlaintext speculativePlaintext) => Promote(speculativePlaintext.Score);
}
