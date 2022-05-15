using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class EntropyTests
{
	[TestMethod]
	public void Zero()
	{
		const string text = "A";
		var result = Entropy.Analyze(text);

		Assert.AreEqual(0, result);
	}
}
