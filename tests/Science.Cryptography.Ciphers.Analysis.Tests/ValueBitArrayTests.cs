using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class ValueBitArrayTests
{
	[TestMethod]
	public void Initial_AllZeros()
	{
		Span<ulong> buffer = stackalloc ulong[2];
		var array = new ValueBitArray(buffer);
		Assert.AreEqual(false, array[0]);
		Assert.AreEqual(false, array[64]);
		Assert.AreEqual(false, array[127]);
	}

	[TestMethod]
	public void Set_Read()
	{
		Span<ulong> buffer = stackalloc ulong[2];
		var array = new ValueBitArray(buffer);
		Assert.AreEqual(false, array[15]);
		array[15] = true;
		Assert.AreEqual(true, array[15]);
		Assert.AreEqual(false, array[14]);
		Assert.AreEqual(false, array[16]);

		Assert.AreEqual(false, array[95]);
		array[95] = true;
		Assert.AreEqual(true, array[95]);
		Assert.AreEqual(false, array[94]);
		Assert.AreEqual(false, array[96]);
	}

	[TestMethod]
	public void Set_Reset()
	{
		Span<ulong> buffer = stackalloc ulong[2];
		var array = new ValueBitArray(buffer);
		Assert.AreEqual(false, array[15]);
		array[15] = true;
		Assert.AreEqual(true, array[15]);
		array.Reset();
		Assert.AreEqual(false, array[15]);
	}

	[TestMethod]
	public void Cardinality()
	{
		Span<ulong> buffer = stackalloc ulong[2];
		var array = new ValueBitArray(buffer);
		Assert.AreEqual(0, array.GetCardinality());

		array[15] = true;
		Assert.AreEqual(1, array.GetCardinality());

		array[95] = true;
		Assert.AreEqual(2, array.GetCardinality());
	}

	[TestMethod]
	public void GetNumberOfBucketsForBits()
	{
		Assert.AreEqual(1, ValueBitArray.GetNumberOfBucketsForBits(12));
		Assert.AreEqual(1, ValueBitArray.GetNumberOfBucketsForBits(63));
		Assert.AreEqual(2, ValueBitArray.GetNumberOfBucketsForBits(65));
	}

	[TestMethod]
	public void SetRange()
	{
		Span<ulong> buffer = stackalloc ulong[3];
		var array = new ValueBitArray(buffer);

		array.SetRangeToOne(3, 3);
		Assert.AreEqual(false, array[0]);
		Assert.AreEqual(false, array[1]);
		Assert.AreEqual(false, array[2]);
		Assert.AreEqual(true, array[3]);
		Assert.AreEqual(true, array[4]);
		Assert.AreEqual(true, array[5]);
		Assert.AreEqual(false, array[6]);
		Assert.AreEqual(false, array[63]);
		Assert.AreEqual(3, array.GetCardinality());

		array.SetRangeToOne(37, 2);
		Assert.AreEqual(false, array[36]);
		Assert.AreEqual(true, array[37]);
		Assert.AreEqual(true, array[38]);
		Assert.AreEqual(false, array[39]);
		Assert.AreEqual(5, array.GetCardinality());
	}
}