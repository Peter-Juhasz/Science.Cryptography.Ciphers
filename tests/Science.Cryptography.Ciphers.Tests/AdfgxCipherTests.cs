using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class AdfgxCipherTests
{
	[TestMethod]
	public void Encrypt()
	{
		var cipher = new AdfgxCipher();
		var polybiusSquare = new PolybiusSquare(new[,]
		{
			{ 'B', 'T', 'A', 'L', 'P' },
			{ 'D', 'H', 'O', 'Z', 'K' },
			{ 'Q', 'F', 'V', 'S', 'N' },
			{ 'G', 'I', 'C', 'U', 'X' },
			{ 'M', 'R', 'E', 'W', 'Y' },
		});
		var transpositionKey = IntArrayKey.FromCharIndexesOfAlphabetSequential("CARGO", WellKnownAlphabets.English, firstIndex: 1);

		const string plaintext = "attackatonce";
		const string ciphertext = "FAXDFADDDGDGFFFAFAXAFAFX";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, (polybiusSquare, transpositionKey)));
	}

	[TestMethod]
	public void Decrypt()
	{
		var cipher = new AdfgxCipher();
		var polybiusSquare = new PolybiusSquare(new[,]
		{
			{ 'B', 'T', 'A', 'L', 'P' },
			{ 'D', 'H', 'O', 'Z', 'K' },
			{ 'Q', 'F', 'V', 'S', 'N' },
			{ 'G', 'I', 'C', 'U', 'X' },
			{ 'M', 'R', 'E', 'W', 'Y' },
		});
		var transpositionKey = IntArrayKey.FromCharIndexesOfAlphabetSequential("CARGO", WellKnownAlphabets.English, firstIndex: 1);

		const string plaintext = "ATTACKATONCE";
		const string ciphertext = "FAXDFADDDGDGFFFAFAXAFAFX";

		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, (polybiusSquare, transpositionKey)));
	}
}
