using Microsoft.VisualStudio.TestTools.UnitTesting;

using Science.Cryptography.Ciphers.Specialized;
using System;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class AsciiRot47CipherTests
{
	[TestMethod]
	public void Rot47()
	{
		var cipher = new Rot47Cipher();

		const string plaintext = "The quick brown fox jumps over the lazy dog";
		const string ciphertext = "%96 BF:4< 3C@H? 7@I ;F>AD @G6C E96 =2KJ 5@8";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
	}

	[TestMethod]
	public void AsciiRot47()
	{
		var cipher = new AsciiRot47Cipher();

		const string plaintext = "The quick brown fox jumps over the lazy dog";
		const string ciphertext = "%96 BF:4< 3C@H? 7@I ;F>AD @G6C E96 =2KJ 5@8";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
	}
}
