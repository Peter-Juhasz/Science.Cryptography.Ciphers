using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class CharsetSpeculativePlaintextRankerTests
{
	[TestMethod]
	public void Rank_Full()
	{
		var ranker = new CharsetSpeculativePlaintextScorer(new[] { 'A', 'B', 'C' }.ToHashSet());
		var result = ranker.Score("ABCCBA");

		Assert.AreEqual(1, result);
	}

	[TestMethod]
	public void Rank()
	{
		var ranker = new CharsetSpeculativePlaintextScorer(new[] { 'A', 'B', 'C' }.ToHashSet());
		var result = ranker.Score("ADBCEF");

		Assert.AreEqual(0.5, result);
	}

	[TestMethod]
	public void Rank_Opposite()
	{
		var ranker = new CharsetSpeculativePlaintextScorer(new[] { 'A', 'B', 'C' }.ToHashSet());
		var result = ranker.Score("XYZ");

		Assert.AreEqual(0, result);
	}
}
