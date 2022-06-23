using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class CharArrayKeyTests
{
	[TestMethod]
	public void FromCharIndexesOfAlphabetSorted()
	{
		var alphabet = WellKnownAlphabets.English;
		var key = "CARGO";
		var result = CharArrayKey.SortByAlphabet(key, alphabet);
		Assert.IsTrue(result.SequenceEqual(new[] { 'A', 'C', 'G', 'O', 'R' }));
	}
}
