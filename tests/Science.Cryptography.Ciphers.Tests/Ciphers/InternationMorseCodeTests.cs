using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class InternationMorseCodeTests
{
	[TestMethod]
	public void Encrypt()
	{
		var cipher = new InternationalMorseCode();

		const string plaintext = "morse code";
		const string ciphertext = "-- --- .-. ... .  -.-. --- -.. .";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
	}

	[TestMethod]
	public void Decrypt()
	{
		var cipher = new InternationalMorseCode();

		const string plaintext = "morse code";
		const string ciphertext = "-- --- .-. ... .  -.-. --- -.. .";

		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext), true);
	}
}
