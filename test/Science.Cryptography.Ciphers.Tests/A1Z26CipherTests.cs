using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class A1Z26CipherTests
{
	[TestMethod]
	public void Encrypt()
	{
		var cipher = new A1Z26Cipher();

		const string plaintext = "The quick brown fox jumps over the lazy dog.";
		const string ciphertext = "20-8-5 17-21-9-3-11 2-18-15-23-14 6-15-24 10-21-13-16-19 15-22-5-18 20-8-5 12-1-26-25 4-15-7.";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
	}

	[TestMethod]
	public void Decrypt()
	{
		var cipher = new A1Z26Cipher();

		const string plaintext = "The quick brown fox jumps over the lazy dog.";
		const string ciphertext = "20-8-5 17-21-9-3-11 2-18-15-23-14 6-15-24 10-21-13-16-19 15-22-5-18 20-8-5 12-1-26-25 4-15-7.";

		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext), ignoreCase: true);
	}
}
