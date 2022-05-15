using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class NGramFrequenciesSpeculativePlaintextScorerTests
{
	[TestMethod]
	public void Score()
	{
		var scorer = new RelativeNGramFrequenciesSpeculativePlaintextScorer(Languages.English.GetNGramFrequencies(2), 2);
		var s = scorer.Score("hello world!");
		Assert.IsTrue(s > 0);
	}

	[TestMethod]
	public void Score_Case()
	{
		var scorer = new RelativeNGramFrequenciesSpeculativePlaintextScorer(Languages.English.GetNGramFrequencies(2), 2);
		Assert.AreEqual(scorer.Score("hello world!"), scorer.Score("HELLO WORLD!"));
	}


	[TestMethod]
	public void Score_TwoAscii()
	{
		var scorer1 = new RelativeNGramFrequenciesSpeculativePlaintextScorer(Languages.English.GetNGramFrequencies(2), 2);
		var scorer2 = new TwoGramAsciiLettersFrequenciesSpeculativePlaintextScorer(Languages.English.GetNGramFrequencies(2));
		var s1 = scorer1.Score("hello world!");
		var s2 = scorer2.Score("hello world!");
		Assert.AreEqual(s1, s2);
	}

	[TestMethod]
	public void Score_TwoAscii_Reuse()
	{
		var scorer = new TwoGramAsciiLettersFrequenciesSpeculativePlaintextScorer(Languages.English.GetNGramFrequencies(2)).GetForPartition();
		var s1 = scorer.Score("hello world!");
		var s2 = scorer.Score("something completely different");
		var s3 = scorer.Score("hello world!");
		Assert.AreNotEqual(s1, s2);
		Assert.AreEqual(s1, s3);
	}

}
