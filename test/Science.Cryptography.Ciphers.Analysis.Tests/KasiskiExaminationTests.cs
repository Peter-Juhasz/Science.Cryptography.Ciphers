using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class KasiskiExaminationTests
{
	[TestMethod]
	public void Analyze()
	{
		string ciphertext = "qvvjcibdftsicicqvvjcibswcstqwmb.";

		var result = KasiskiExamination.Analyze(ciphertext);
		CollectionAssert.AreEquivalent(new[] { 3, 5 }, result.SpeculativeKeyLengths.ToArray());
	}

	[TestMethod]
	public void Analyze_IgnoreCase()
	{
		string ciphertext = "QvvjcibdftsicicqvvJcibswcstqwmb.";

		var result = KasiskiExamination.Analyze(ciphertext, comparison: StringComparison.OrdinalIgnoreCase);
		CollectionAssert.AreEquivalent(new[] { 3, 5 }, result.SpeculativeKeyLengths.ToArray());
	}
}
