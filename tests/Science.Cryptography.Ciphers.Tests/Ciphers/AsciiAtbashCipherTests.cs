using Microsoft.VisualStudio.TestTools.UnitTesting;

using Science.Cryptography.Ciphers.Specialized;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class AsciiAtbashCipherTests
{
	[TestMethod]
	public void AsciiAtbash()
	{
		var cipher = new AsciiAtbashCipher();

		const string plaintext = "AbcdefghijklmnopqrstuvwxyzZYX";
		const string ciphertext = "ZyxwvutsrqponmlkjihgfedcbaABC";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
	}
}
