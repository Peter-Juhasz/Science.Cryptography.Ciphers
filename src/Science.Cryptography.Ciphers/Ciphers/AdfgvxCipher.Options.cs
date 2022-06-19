namespace Science.Cryptography.Ciphers;

public record struct AdfgvxCipherOptions(
	char[] Labels
)
{
	public static readonly AdfgvxCipherOptions ADFGX = new(new[] { 'A', 'D', 'F', 'G', 'X' });

	public static readonly AdfgvxCipherOptions ADFGVX = new(new[] { 'A', 'D', 'F', 'G', 'V', 'X' });
}
