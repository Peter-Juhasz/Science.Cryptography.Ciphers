using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class WordlistSpeculativePlaintextRankerTests
{
	[TestMethod]
	public void Rank_Full()
	{
		var ranker = new WordlistSpeculativePlaintextScorer(new[] 
		{
			"the",
			"answer",
			"is",
			"wordlist",
		});
		var result = ranker.Score("theansweriswordlist");

		Assert.AreEqual(1, result);
	}

	[TestMethod]
	public void Rank()
	{
		var ranker = new WordlistSpeculativePlaintextScorer(new[] 
		{
			"the",
			"answer",
			"is",
			"wordlist",
		});
		var result = ranker.Score("the answer is: wordlist.");

		Assert.AreEqual(19 / 24D, result);
	}

	[TestMethod]
	public void Rank_CaseInsensitive()
	{
		var ranker = new WordlistSpeculativePlaintextScorer(new[] 
		{
			"the",
			"answeR",
			"is",
			"Wordlist",
		});
		var result = ranker.Score("The answer Is: wordList.");

		Assert.AreEqual(19 / 24D, result);
	}

	[TestMethod]
	public void Rank_Opposite()
	{
		var ranker = new WordlistSpeculativePlaintextScorer(new[]
		{
			"the",
			"answer",
			"is",
			"wordlist",
		});
		var result = ranker.Score("does not match.");

		Assert.AreEqual(0, result);
	}
}
