using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class FrequencyAnalysisTests
{
	[TestMethod]
	public void Compare_Identical()
	{
		var result = FrequencyAnalysis.Compare(
			new RelativeCharacterFrequencies(new Dictionary<char, double>
			{
				{ 'A', 0.2 },
				{ 'B', 0.5 },
				{ 'C', 0.3 },
			}),
			new RelativeCharacterFrequencies(new Dictionary<char, double>
			{
				{ 'A', 0.2 },
				{ 'B', 0.5 },
				{ 'C', 0.3 },
			})
		);

		Assert.AreEqual(1, result);
	}

	[TestMethod]
	public void Compare_Opposite()
	{
		var result = FrequencyAnalysis.Compare(
			new RelativeCharacterFrequencies(new Dictionary<char, double>
			{
				{ 'A', 0.2 },
				{ 'B', 0.8 },
			}),
			new RelativeCharacterFrequencies(new Dictionary<char, double>
			{
				{ 'C', 0.3 },
				{ 'D', 0.7 },
			})
		);

		Assert.AreEqual(0, result);
	}
}
