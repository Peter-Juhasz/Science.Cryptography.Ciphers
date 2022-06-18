using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class ColumnarTranspositionCipherTests
{
	[TestMethod]
	public void Encrypt()
	{
		var cipher = new ColumnarTranspositionCipher();
		var key = new[] { 6, 3, 2, 4, 1, 5 };

		const string plaintext = "WEAREDISCOVEREDFLEEATONCE";
		const string ciphertext = "EVLNACDTESEAROFODEECWIREE";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
	}

	[TestMethod]
	public void Decrypt()
	{
		var cipher = new ColumnarTranspositionCipher();
		var key = new[] { 6, 3, 2, 4, 1, 5 };

		const string plaintext = "WEAREDISCOVEREDFLEEATONCE";
		const string ciphertext = "EVLNACDTESEAROFODEECWIREE";

		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
	}
}
