namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents a key for the <see cref="AffineCipher" />.
/// </summary>
public record struct AffineKey(int A, int B);
