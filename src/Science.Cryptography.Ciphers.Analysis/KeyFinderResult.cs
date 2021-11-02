namespace Science.Cryptography.Ciphers.Analysis;

public record class KeyFinderResult<TKey>(
    TKey Key,
    string SpeculativePlaintext,
    double Score
)
{
}
