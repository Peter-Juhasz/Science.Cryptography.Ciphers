using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class StraddlingCheckerboardCipherTests
{
	[TestMethod]
	public void Encrypt()
	{
		var cipher = new StraddlingCheckerboardCipher();

		const string plaintext = "ATTACK AT DAWN";
		const string ciphertext = "31132127 31 223655";
		var square = StraddlingCheckerboard.Create(new char[,]
		{
			{ 'E', 'T', StraddlingCheckerboard.EmptyValue, 'A', 'O', 'N', StraddlingCheckerboard.EmptyValue, 'R', 'I', 'S' },
			{ 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M' },
			{ 'P', 'Q', StraddlingCheckerboard.NumericEscape, 'U', 'V', 'W', 'X', 'Y', 'Z', StraddlingCheckerboard.FullStop },
		}, 2, 6);

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, square));
	}

	[TestMethod]
	public void Decrypt()
	{
		var cipher = new StraddlingCheckerboardCipher();

		const string plaintext = "ATTACK AT DAWN";
		const string ciphertext = "31132127 31 223655";
		var square = StraddlingCheckerboard.Create(new char[,]
		{
			{ 'E', 'T', StraddlingCheckerboard.EmptyValue, 'A', 'O', 'N', StraddlingCheckerboard.EmptyValue, 'R', 'I', 'S' },
			{ 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M' },
			{ 'P', 'Q', StraddlingCheckerboard.NumericEscape, 'U', 'V', 'W', 'X', 'Y', 'Z', StraddlingCheckerboard.FullStop },
		}, 2, 6);

		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, square));
	}
}
