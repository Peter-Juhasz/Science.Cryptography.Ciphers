using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class PolybiusCipherTests
{
	[TestMethod]
	public void Encrypt()
	{
		var cipher = new PolybiusCipher();

		const string plaintext = "HELLO WORLD";
		const string ciphertext = "3331121242 5342221225";
		var square = PolybiusSquare.CreateFromKeyword("PLAYFAIR", WellKnownAlphabets.EnglishWithoutJ);

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, square));
	}

	[TestMethod]
	public void Decrypt()
	{
		var cipher = new PolybiusCipher();

		const string plaintext = "HELLO WORLD";
		const string ciphertext = "3331121242 5342221225";
		var square = PolybiusSquare.CreateFromKeyword("PLAYFAIR", WellKnownAlphabets.EnglishWithoutJ);

		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, square));
	}
}
