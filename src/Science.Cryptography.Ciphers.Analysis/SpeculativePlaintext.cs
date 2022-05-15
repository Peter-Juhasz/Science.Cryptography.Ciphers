namespace Science.Cryptography.Ciphers.Analysis;

public record class SpeculativePlaintext(
	string Plaintext,
	double Score,
	ISpeculativePlaintextScorer Scorer
);
