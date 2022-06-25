using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class RelativeFirstLetterOfWordsFrequenciesSpeculativePlaintextRankerTests
{
	[TestMethod]
	public void Analyze()
	{
		var result = RelativeFirstLetterOfWordsFrequenciesSpeculativePlaintextScorer.Analyze("these three words");
		Assert.AreEqual(2, result.ToDictionary().Count);
		Assert.AreEqual(2, result['t']);
		Assert.AreEqual(1, result['w']);
	}
}
