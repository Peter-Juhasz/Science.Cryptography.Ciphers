using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class ArrayKeySpaceTests
{
	[TestMethod]
	public void MyTestMethod()
	{
		var keySpace = new ArrayKeySpace<int>(1, 3, new [] { 5, 6, 7 }.ToHashSet());
		
		var enumerator = keySpace.GetPartitions().GetEnumerator();
		
		enumerator.MoveNext();
		var partition1 = enumerator.Current.GetKeys().GetEnumerator();
		partition1.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5 }, partition1.Current);
		Assert.IsFalse(partition1.MoveNext());

		enumerator.MoveNext();
		var partition2 = enumerator.Current.GetKeys().GetEnumerator();
		partition2.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6 }, partition2.Current);
		Assert.IsFalse(partition2.MoveNext());

		enumerator.MoveNext();
		var partition3 = enumerator.Current.GetKeys().GetEnumerator();
		partition3.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7 }, partition3.Current);
		Assert.IsFalse(partition3.MoveNext());

		enumerator.MoveNext();
		var partition4 = enumerator.Current.GetKeys().GetEnumerator();
		partition4.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 5 }, partition4.Current);
		partition4.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 6 }, partition4.Current);
		partition4.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 7 }, partition4.Current);
		Assert.IsFalse(partition4.MoveNext());

		enumerator.MoveNext();
		var partition5 = enumerator.Current.GetKeys().GetEnumerator();
		partition5.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 5 }, partition5.Current);
		partition5.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 6 }, partition5.Current);
		partition5.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 7 }, partition5.Current);
		Assert.IsFalse(partition5.MoveNext());

		enumerator.MoveNext();
		var partition6 = enumerator.Current.GetKeys().GetEnumerator();
		partition6.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 5 }, partition6.Current);
		partition6.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 6 }, partition6.Current);
		partition6.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 7 }, partition6.Current);
		Assert.IsFalse(partition6.MoveNext());

		enumerator.MoveNext();
		var partition7 = enumerator.Current.GetKeys().GetEnumerator();
		partition7.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 5, 5 }, partition7.Current);
		partition7.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 5, 6 }, partition7.Current);
		partition7.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 5, 7 }, partition7.Current);
		partition7.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 6, 5 }, partition7.Current);
		partition7.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 6, 6 }, partition7.Current);
		partition7.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 6, 7 }, partition7.Current);
		partition7.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 7, 5 }, partition7.Current);
		partition7.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 7, 6 }, partition7.Current);
		partition7.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 5, 7, 7 }, partition7.Current);
		Assert.IsFalse(partition7.MoveNext());

		enumerator.MoveNext();
		var partition8 = enumerator.Current.GetKeys().GetEnumerator();
		partition8.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 5, 5 }, partition8.Current);
		partition8.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 5, 6 }, partition8.Current);
		partition8.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 5, 7 }, partition8.Current);
		partition8.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 6, 5 }, partition8.Current);
		partition8.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 6, 6 }, partition8.Current);
		partition8.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 6, 7 }, partition8.Current);
		partition8.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 7, 5 }, partition8.Current);
		partition8.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 7, 6 }, partition8.Current);
		partition8.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 6, 7, 7 }, partition8.Current);
		Assert.IsFalse(partition8.MoveNext());

		enumerator.MoveNext();
		var partition9 = enumerator.Current.GetKeys().GetEnumerator();
		partition9.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 5, 5 }, partition9.Current);
		partition9.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 5, 6 }, partition9.Current);
		partition9.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 5, 7 }, partition9.Current);
		partition9.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 6, 5 }, partition9.Current);
		partition9.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 6, 6 }, partition9.Current);
		partition9.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 6, 7 }, partition9.Current);
		partition9.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 7, 5 }, partition9.Current);
		partition9.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 7, 6 }, partition9.Current);
		partition9.MoveNext();
		CollectionAssert.AreEquivalent(new[] { 7, 7, 7 }, partition9.Current);
		Assert.IsFalse(partition9.MoveNext());

		Assert.IsFalse(enumerator.MoveNext());
	}
}
