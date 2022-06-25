using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class RegexSpeculativePlaintextRankerTests
{
	[TestMethod]
	public void Rank_Match()
	{
		var ranker = new RegexSpeculativePlaintextScorer(new Regex(@"the \w+ is .*", RegexOptions.IgnoreCase));
		var result = ranker.Score("The secret is 42.");

		Assert.AreEqual(1, result);
	}

	[TestMethod]
	public void Rank_DoesNotMatch()
	{
		var ranker = new RegexSpeculativePlaintextScorer(new Regex(@"the \w+ is .*", RegexOptions.IgnoreCase));
		var result = ranker.Score("Does not match.");

		Assert.AreEqual(0, result);
	}
}
