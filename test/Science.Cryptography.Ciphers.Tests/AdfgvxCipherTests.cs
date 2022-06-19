using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class AdfgvxCipherTests
{
	[TestMethod]
	public void Encrypt()
	{
		var cipher = new AdfgvxCipher();
		var polybiusSquare = new PolybiusSquare(new[,]
		{
			{ 'N', 'A', '1', 'C', '3', 'H' },
			{ '8', 'T', 'B', '2', 'O', 'M' },
			{ 'E', '5', 'W', 'R', 'P', 'D' },
			{ '4', 'F', '6', 'G', '7', 'I' },
			{ '9', 'J', '0', 'K', 'L', 'Q' },
			{ 'S', 'U', 'V', 'X', 'Y', 'Z' },
		});
		var transpositionKey = new[] { 4, 5, 3, 6, 1, 2, 7 };

		const string plaintext = "ATTACKAT1200AM";
		const string ciphertext = "DGDDDAGDDGAFADDFDADVDVFAADVX";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, (polybiusSquare, transpositionKey)));
	}

	[TestMethod]
	public void Decrypt()
	{
		var cipher = new AdfgvxCipher();
		var polybiusSquare = new PolybiusSquare(new[,]
		{
			{ 'N', 'A', '1', 'C', '3', 'H' },
			{ '8', 'T', 'B', '2', 'O', 'M' },
			{ 'E', '5', 'W', 'R', 'P', 'D' },
			{ '4', 'F', '6', 'G', '7', 'I' },
			{ '9', 'J', '0', 'K', 'L', 'Q' },
			{ 'S', 'U', 'V', 'X', 'Y', 'Z' },
		});
		var transpositionKey = new[] { 4, 5, 3, 6, 1, 2, 7 };

		const string plaintext = "ATTACKAT1200AM";
		const string ciphertext = "DGDDDAGDDGAFADDFDADVDVFAADVX";

		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, (polybiusSquare, transpositionKey)));
	}
}
