using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class PlayfairSquareTests
{
	[TestMethod]
	public void FastFill()
	{
		var reference = new char[5, 5];
		ArrayHelper.FillSlow(reference, "PLAYFIREXMBCDGHKNOQSTUVWZ", 5);

		var fast = new char[5, 5];
		ArrayHelper.FillFast(fast, "PLAYFIREXMBCDGHKNOQSTUVWZ", 5);

		CollectionAssert.AreEquivalent(reference, fast);
	}

	[TestMethod]
	public void CreateFromString()
	{
		var square = PolybiusSquare.CreateFromCharacters("PLAYFIREXMBCDGHKNOQSTUVWZ");
		Assert.AreEqual('P', square[0, 0]);
		Assert.AreEqual('N', square[3, 1]);
		Assert.AreEqual('D', square[2, 2]);
		Assert.AreEqual('M', square[1, 4]);
		Assert.AreEqual('Z', square[4, 4]);
	}

	[TestMethod]
	public void Resolve_DifferentColumnRow()
	{
		var square = PolybiusSquare.CreateFromCharacters("PLAYFIREXMBCDGHKNOQSTUVWZ");
		PlayfairCipher.TryResolveEncrypt(square, ('H', 'I'), out (char c1, char c2) position);
		Assert.AreEqual(('B', 'M'), position);
	}

	[TestMethod]
	public void Resolve_SameRow()
	{
		var square = PolybiusSquare.CreateFromCharacters("PLAYFIREXMBCDGHKNOQSTUVWZ");
		PlayfairCipher.TryResolveEncrypt(square, ('E', 'X'), out (char c1, char c2) position);
		Assert.AreEqual(('X', 'M'), position);
	}

	[TestMethod]
	public void Resolve_SameColumn()
	{
		var square = PolybiusSquare.CreateFromCharacters("PLAYFIREXMBCDGHKNOQSTUVWZ");
		Assert.IsTrue(PlayfairCipher.TryResolveEncrypt(square, ('E', 'D'), out (char c1, char c2) position));
		Assert.AreEqual(('D', 'O'), position);
		Assert.IsTrue(PlayfairCipher.TryResolveEncrypt(square, ('D', 'E'), out position));
		Assert.AreEqual(('O', 'D'), position);
		Assert.IsTrue(PlayfairCipher.TryResolveEncrypt(square, ('A', 'O'), out position));
		Assert.AreEqual(('E', 'V'), position);
	}
}
