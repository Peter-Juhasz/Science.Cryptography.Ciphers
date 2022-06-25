using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class SubstringSpeculativePlaintextRankerTests
{
	[TestMethod]
	public void Rank_Match()
	{
		var ranker = new SubstringSpeculativePlaintextScorer("secret");
		var result = ranker.Score("The secret is 42.");

		Assert.AreEqual(1, result);
	}

	[TestMethod]
	public void Rank_DoesNotMatch()
	{
		var ranker = new SubstringSpeculativePlaintextScorer("secret");
		var result = ranker.Score("The answer is 42.");

		Assert.AreEqual(0, result);
	}
}
