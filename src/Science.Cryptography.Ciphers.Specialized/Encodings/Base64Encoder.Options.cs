namespace Science.Cryptography.Ciphers.Specialized;

public record class Base64Options(
	char Padding = '=',
	char Plus = '+',
	char Slash = '/'
)
{
	public static readonly Base64Options Default = new();

	public static readonly Base64Options Url = new(Plus: '-', Slash: '_');

	public static readonly Base64Options Imap = new(Plus: '+', Slash: ',');
}
