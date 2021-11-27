namespace Science.Cryptography.Ciphers.Specialized;

public class Base64Options
{
    public char Padding { get; set; } = '=';

    public char Plus { get; set; } = '+';

    public char Slash { get; set; } = '/';

    public static readonly Base64Options Default = new();
}
