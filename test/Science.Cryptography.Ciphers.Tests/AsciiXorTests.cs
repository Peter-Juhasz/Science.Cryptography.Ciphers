using Microsoft.VisualStudio.TestTools.UnitTesting;

using Science.Cryptography.Ciphers.Specialized;

using System.Text;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
public class AsciiXorTests
{
	[TestMethod]
	public void AsciiXor_KeyShorterThanVector()
	{
		var cipher = new AsciiXorCipher();

		const string plaintext = "thequickbrownfoxjumpsoverthelazydoggodyzalehtrevospmujxofnworbkciuqeht";
		const string ciphertext = @"EZVE@_TZPA[BXQ^JYAXFD^DVFA^R]SIMQYPV]WMOW[TZGFP@XBB^A_NXW\D[GT\R[FEP^C";
		const string key = "1234567";
		var keyBytes = Encoding.ASCII.GetBytes(key);

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, keyBytes));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, keyBytes));
	}

	[TestMethod]
	public void AsciiXor_KeyLongerThanVector()
	{
		var cipher = new AsciiXorCipher();

		const string plaintext = "thequickbrownfoxjumpsoverthelazydoggodyzalehtrevospmujxofnworbkciuqeht";
		const string ciphertext = @"EZR@BZZSSAWD[^\NRDTFA^@UKM^T_SIN\\^U\]NKP^RYCA\N^@H^@RKY^_NY@S]SPLGT[F";
		const string key = "1271739813835836819621609961323783923971";
		var keyBytes = Encoding.ASCII.GetBytes(key);

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, keyBytes));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, keyBytes));
	}

	[TestMethod]
	public void AsciiXor_KeySameSizeAsVector()
	{
		var cipher = new AsciiXorCipher();

		const string plaintext = "thequickbrownfoxjumpsoverthelazydoggodyzalehtrevospmujxofnworbkciuqeht";
		const string ciphertext = @"EZR@BZZSSAWD[^\NRDTFA^@UKM^T_SINU]PVXW@BP_][AJV@WBI[G[N__WA^APXTXGFT_G";
		const string key = "12717398138358368196216099613237";
		var keyBytes = Encoding.ASCII.GetBytes(key);

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, keyBytes));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, keyBytes));
	}
}
