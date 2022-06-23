using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class IntArrayKeyTests
{
	[TestMethod]
	public void TryFromCharIndexesOfAlphabet()
	{
		var alphabet = WellKnownAlphabets.English;
		var key = "ABCDEF";
		var result = IntArrayKey.FromCharIndexesOfAlphabet(key, alphabet, firstIndex: 1);
		Assert.IsTrue(result.SequenceEqual(new[] { 1, 2, 3, 4, 5, 6 }));
	}

	[TestMethod]
	public void FromCharIndexesOfAlphabetSorted()
	{
		var alphabet = WellKnownAlphabets.English;
		var key = "CARGO";
		var result = IntArrayKey.FromCharIndexesOfAlphabetSorted(key, alphabet, firstIndex: 1);
		Assert.IsTrue(result.SequenceEqual(new[] { 1, 3, 7, 15, 18 }));
	}

	[TestMethod]
	public void FromCharIndexesOfAlphabetSequential()
	{
		var alphabet = WellKnownAlphabets.English;
		var key = "CARGO";
		var result = IntArrayKey.FromCharIndexesOfAlphabetSequential(key, alphabet, firstIndex: 1);
		Assert.IsTrue(result.SequenceEqual(new[] { 2, 1, 5, 3, 4 }));
	}

	[TestMethod]
	public void FromCharIndexesOfAlphabetSequential_Duplicates()
	{
		var alphabet = WellKnownAlphabets.English;
		var key = "CARGGO";
		var result = IntArrayKey.FromCharIndexesOfAlphabetSequential(key, alphabet, firstIndex: 1);
		Assert.IsTrue(result.SequenceEqual(new[] { 2, 1, 5, 3, 3, 4 }));
	}
}
