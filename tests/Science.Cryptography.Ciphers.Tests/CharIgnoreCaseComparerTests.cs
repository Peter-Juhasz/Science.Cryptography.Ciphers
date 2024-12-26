using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class AlphabetTests
{
	[TestMethod]
	public void AtMod_Regular()
	{
		Assert.AreEqual('A', WellKnownAlphabets.English.AtMod(0));
		Assert.AreEqual('B', WellKnownAlphabets.English.AtMod(1));
	}

	[TestMethod]
	public void AtMod_Overflow()
	{
		Assert.AreEqual('A', WellKnownAlphabets.English.AtMod(26));
		Assert.AreEqual('B', WellKnownAlphabets.English.AtMod(27));
		Assert.AreEqual('A', WellKnownAlphabets.English.AtMod(52));
	}

	[TestMethod]
	public void AtMod_Negative()
	{
		Assert.AreEqual('Z', WellKnownAlphabets.English.AtMod(-1));
		Assert.AreEqual('A', WellKnownAlphabets.English.AtMod(-26));
		Assert.AreEqual('Z', WellKnownAlphabets.English.AtMod(-27));
	}
}

[TestClass]
public class CharIgnoreCaseComparerTests
{
	[TestMethod]
	public void Compare_SameCharacterAndSameCase()
	{
		Assert.IsTrue(IgnoreCaseCharComparer.Instance.Equals('A', 'A'));
	}

	[TestMethod]
	public void Compare_SameCharacterAndDifferentCase()
	{
		Assert.IsTrue(IgnoreCaseCharComparer.Instance.Equals('A', 'a'));
		Assert.IsTrue(IgnoreCaseCharComparer.Instance.Equals('a', 'A'));
	}

	[TestMethod]
	public void Compare_DifferentCharacter()
	{
		Assert.IsFalse(IgnoreCaseCharComparer.Instance.Equals('A', 'B'));
		Assert.IsFalse(IgnoreCaseCharComparer.Instance.Equals('A', 'b'));
		Assert.IsFalse(IgnoreCaseCharComparer.Instance.Equals('a', 'B'));
	}

	[TestMethod]
	public void GetHashCode_SameCharacterAndSameCase()
	{
		Assert.AreEqual(IgnoreCaseCharComparer.Instance.GetHashCode('A'), IgnoreCaseCharComparer.Instance.GetHashCode('A'));
	}

	[TestMethod]
	public void GetHashCode_SameCharacterAndDifferentCase()
	{
		Assert.AreEqual(IgnoreCaseCharComparer.Instance.GetHashCode('A'), IgnoreCaseCharComparer.Instance.GetHashCode('a'));
	}

	[TestMethod]
	public void GetHashCode_DifferentCharacter()
	{
		Assert.AreNotEqual(IgnoreCaseCharComparer.Instance.GetHashCode('A'), IgnoreCaseCharComparer.Instance.GetHashCode('B'));
		Assert.AreNotEqual(IgnoreCaseCharComparer.Instance.GetHashCode('A'), IgnoreCaseCharComparer.Instance.GetHashCode('b'));
		Assert.AreNotEqual(IgnoreCaseCharComparer.Instance.GetHashCode('a'), IgnoreCaseCharComparer.Instance.GetHashCode('B'));
	}
}
