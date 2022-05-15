namespace Science.Cryptography.Ciphers.Specialized;

public record struct BinaryOptions(
	char Zero = '0',
	char One = '1'
)
{
	public static readonly BinaryOptions Default = new(Zero: '0', One: '1');
}
