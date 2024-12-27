using Microsoft.VisualStudio.TestTools.UnitTesting;

using Science.Cryptography.Ciphers.Specialized;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class AtbashCipherTests
{
	[TestMethod]
	public void Atbash()
	{
		var cipher = new AtbashCipher();

		const string plaintext = "Abcdefghijklmnopqrstuvwxyz";
		const string ciphertext = "Zyxwvutsrqponmlkjihgfedcba";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
	}
}
