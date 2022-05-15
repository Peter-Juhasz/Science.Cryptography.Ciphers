namespace Science.Cryptography.Ciphers;

/// <summary>
/// Contains a set of alphabets.
/// </summary>
public static class WellKnownAlphabets
{
	/// <summary>
	/// The regular english alphabet.
	/// </summary>
	public static readonly Alphabet English = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	public static readonly Alphabet EnglishWithoutI = "ABCDEFGHJKLMNOPQRSTUVWXYZ";

	public static readonly Alphabet EnglishWithoutJ = "ABCDEFGHIKLMNOPQRSTUVWXYZ";

	public static readonly Alphabet EnglishWithoutK = "ABCDEFGHIJLMNOPQRSTUVWXYZ";

	public static readonly Alphabet EnglishWithoutL = "ABCDEFGHIJKMNOPQRSTUVWXYZ";

	public static readonly Alphabet EnglishWithoutQ = "ABCDEFGHIJKLMNOPRSTUVWXYZ";


	public static readonly Alphabet UppercaseEnglish = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	public static readonly Alphabet LowercaseEnglish = "abcdefghijklmnopqrstuvwxyz";


	public static readonly Alphabet DigitsFrom0To9 = "0123456789";

	public static readonly Alphabet DigitsFrom1To0 = "1234567890";
}
