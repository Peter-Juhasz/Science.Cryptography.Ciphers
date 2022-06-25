using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class KeyFinderTests
{
	[TestMethod]
	public void FindBest_Shift_Substring()
	{
		string ciphertext = "Znk ykixkz oy Igkygx.";

		var result = KeyFinder.SolveForBest(
			ciphertext,
			new ShiftCipher(),
			new ShiftKeySpace(WellKnownAlphabets.English),
			new SubstringSpeculativePlaintextScorer("secret")
		);
		Assert.IsNotNull(result);

		Assert.AreEqual(6, result.Key);
		Assert.AreEqual(1.0, result.SpeculativePlaintext.Score);
		Assert.AreEqual("The secret is Caesar.", result.SpeculativePlaintext.Plaintext);
	}

	[TestMethod]
	public void FindBest_NoMatch()
	{
		string ciphertext = "Znk ykixkz oy Igkygx.";

		var result = KeyFinder.SolveForBest(
			ciphertext,
			new ShiftCipher(),
			new ShiftKeySpace(WellKnownAlphabets.English),
			new SubstringSpeculativePlaintextScorer("answer")
		);

		Assert.IsNull(result);
	}
}
