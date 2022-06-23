namespace Science.Cryptography.Ciphers;

public record struct InternationalMorseCodeOptions(
	char Dot = '.',
	char Dash = '-',
	char Delimiter = ' '
)
{
	public static readonly InternationalMorseCodeOptions Default = new(Dot: '.', Dash: '-', Delimiter: ' ');
}