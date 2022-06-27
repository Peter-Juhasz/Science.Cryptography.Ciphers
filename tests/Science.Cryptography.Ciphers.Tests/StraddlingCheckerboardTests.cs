using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class StraddlingCheckerboardTests
{
	[TestMethod]
	public void CreateFromArray()
	{
		var sc = StraddlingCheckerboard.Create(new char[,]
		{
			{ 'E', 'T', StraddlingCheckerboard.EmptyValue, 'A', 'O', 'N', StraddlingCheckerboard.EmptyValue, 'R', 'I', 'S' },
			{ 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M' },
			{ 'P', 'Q', StraddlingCheckerboard.NumericEscape, 'U', 'V', 'W', 'X', 'Y', 'Z', StraddlingCheckerboard.FullStop },
		}, 2, 6);
		Assert.AreEqual(3, sc.Height);
		Assert.AreEqual('E', sc[StraddlingCheckerboard.EmptyIndex, 0]);
		Assert.AreEqual('T', sc[StraddlingCheckerboard.EmptyIndex, 1]);
		Assert.AreEqual('G', sc[2, 4]);
		Assert.AreEqual('U', sc[6, 3]);
	}

	[TestMethod]
	public void TryFindOffsets()
	{
		var sc = StraddlingCheckerboard.Create(new char[,]
		{
			{ 'E', 'T', StraddlingCheckerboard.EmptyValue, 'A', 'O', 'N', StraddlingCheckerboard.EmptyValue, 'R', 'I', 'S' },
			{ 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M' },
			{ 'P', 'Q', StraddlingCheckerboard.NumericEscape, 'U', 'V', 'W', 'X', 'Y', 'Z', StraddlingCheckerboard.FullStop },
		}, 2, 6);
		Assert.IsTrue(sc.TryFindOffsets('Z', out var position));
		Assert.AreEqual((6, 8), position);
	}
}
