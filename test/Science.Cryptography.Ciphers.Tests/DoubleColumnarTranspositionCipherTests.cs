using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class DoubleColumnarTranspositionCipherTests
{
	[TestMethod]
	public void Encrypt()
	{
		var cipher = new DoubleColumnarTranspositionCipher();
		var key1 = new[] { 6, 3, 2, 4, 1, 5 };
		var key2 = new[] { 5, 6, 4, 2, 3, 1 };

		const string plaintext = "WEAREDISCOVEREDFLEEATONCE";
		const string ciphertext = "CAEENSOIAEDRLEFWEDREEVTOC";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, (key1, key2)));
	}

	[TestMethod]
	public void Decrypt()
	{
		var cipher = new DoubleColumnarTranspositionCipher();
		var key1 = new[] { 6, 3, 2, 4, 1, 5 };
		var key2 = new[] { 5, 6, 4, 2, 3, 1 };

		const string plaintext = "WEAREDISCOVEREDFLEEATONCE";
		const string ciphertext = "CAEENSOIAEDRLEFWEDREEVTOC";

		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, (key1, key2)));
	}
}
