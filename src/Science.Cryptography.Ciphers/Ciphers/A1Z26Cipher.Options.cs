namespace Science.Cryptography.Ciphers;

public record struct A1Z26Options(char Separator)
{
	public static A1Z26Options Default { get; } = new(Separator: '-');
}
