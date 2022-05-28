namespace Science.Cryptography.Ciphers;

public record struct MorseCodeOptions(
	char Dot = '.',
	char Dash = '-',
	char Delimiter = ' '
)
{
	public static readonly MorseCodeOptions Default = new(Dot: '.', Dash: '-', Delimiter: ' ');
}