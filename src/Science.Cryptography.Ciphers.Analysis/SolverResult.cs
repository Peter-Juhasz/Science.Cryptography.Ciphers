namespace Science.Cryptography.Ciphers.Analysis;

public record class SolverResult(
	SpeculativePlaintext SpeculativePlaintext,
	PlainOrKeyedCipher Cipher
);
