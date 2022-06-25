using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class RelativeLetterFrequenciesSpeculativePlaintextScorerTests
{
	[TestMethod]
	public void Score()
	{
		var scorer = new RelativeLetterFrequenciesSpeculativePlaintextScorer(Languages.English.RelativeFrequenciesOfLetters);
		var s = scorer.Score("hello world!");
		Assert.IsTrue(s > 0);
	}

	[TestMethod]
	public void Score_Case()
	{
		var scorer = new RelativeLetterFrequenciesSpeculativePlaintextScorer(Languages.English.RelativeFrequenciesOfLetters);
		Assert.AreEqual(scorer.Score("hello world!"), scorer.Score("HELLO WORLD!"));
	}
}
