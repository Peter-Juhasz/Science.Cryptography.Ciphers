namespace Science.Cryptography.Ciphers.Analysis;

public class PlainOrKeyedCipher
{
	public PlainOrKeyedCipher(ICipher cipher)
	{
		Cipher = cipher;
		HasKey = false;
	}

	public PlainOrKeyedCipher(object keyedCipher, object key)
	{
		Cipher = keyedCipher;
		Key = key;
		HasKey = true;
	}

	public object Cipher { get; }

	public object? Key { get; }

	public bool HasKey { get; }
}