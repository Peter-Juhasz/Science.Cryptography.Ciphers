using Microsoft.VisualStudio.TestTools.UnitTesting;

using Science.Cryptography.Ciphers.Specialized;
using System;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class AsciiShiftCipherTests
{
	[TestMethod]
	public void Caesar()
	{
		var cipher = new AsciiShiftCipher();

		const string plaintext = "The quick brown fox jumps over the lazy dog.";
		const string ciphertext = "Wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj.";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, WellKnownShiftCipherKeys.Caesar));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, WellKnownShiftCipherKeys.Caesar));
	}

	[TestMethod]
	public void Rot13()
	{
		var cipher = new AsciiShiftCipher();

		const string plaintext = "Why did the chicken cross the road?";
		const string ciphertext = "Jul qvq gur puvpxra pebff gur ebnq?";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, WellKnownShiftCipherKeys.Rot13));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, WellKnownShiftCipherKeys.Rot13));
	}
}
