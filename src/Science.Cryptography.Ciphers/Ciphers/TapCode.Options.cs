namespace Science.Cryptography.Ciphers;

public class TapCodeOptions
{
	public char Dot { get; set; } = '.';

	public char Delimiter { get; set; } = ' ';

	public static readonly TapCodeOptions Default = new();
}