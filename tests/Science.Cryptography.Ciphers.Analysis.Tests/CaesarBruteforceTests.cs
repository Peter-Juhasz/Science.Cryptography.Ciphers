using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class CaesarBruteforceTests
{
	[TestMethod]
	public void NumberOfElements()
	{
		const string text = "the quick brown fox jumps over the lazy dog";
		var result = CaesarBruteforce.Analyze(text, WellKnownAlphabets.English);
		Assert.AreEqual(26, result.Count);
	}
}
