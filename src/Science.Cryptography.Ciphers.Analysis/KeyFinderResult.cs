namespace Science.Cryptography.Ciphers.Analysis;

public record class KeyFinderResult<TKey>(
	TKey Key,
	SpeculativePlaintext SpeculativePlaintext
)
{
}
