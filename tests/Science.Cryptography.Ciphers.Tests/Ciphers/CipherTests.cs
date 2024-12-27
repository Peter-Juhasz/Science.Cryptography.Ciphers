using Microsoft.VisualStudio.TestTools.UnitTesting;

using Science.Cryptography.Ciphers.Specialized;
using System;

namespace Science.Cryptography.Ciphers.Tests;

[TestClass]
[Obsolete("Encapsulate tests for each cipher into a separate class.")]
public class CipherTests
{
	[TestMethod]
	public void Caesar()
	{
		var cipher = new ShiftCipher();

		const string plaintext = "The quick brown fox jumps over the lazy dog.";
		const string ciphertext = "Wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj.";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, WellKnownShiftCipherKeys.Caesar));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, WellKnownShiftCipherKeys.Caesar));
	}

	[TestMethod]
	public void Rot13()
	{
		var cipher = new ShiftCipher();

		const string plaintext = "Why did the chicken cross the road?";
		const string ciphertext = "Jul qvq gur puvpxra pebff gur ebnq?";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, WellKnownShiftCipherKeys.Rot13));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, WellKnownShiftCipherKeys.Rot13));
	}

	[TestMethod]
	public void TapCode()
	{
		var cipher = new TapCode();

		const string plaintext = "WATER";
		const string ciphertext = "..... .. . . .... .... . ..... .... ..";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext), true);
	}

	[TestMethod]
	public void Multiplicative_Encrypt()
	{
		var cipher = new MultiplicativeCipher();

		const string plaintext = "GEHEIMNIS";
		const string ciphertext = "SMVMYKNYC";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, 3));
	}

	/*
    [TestMethod]
    public void Bifid()
    {
        var cipher = new BifidCipher();

        char[,] key = new char[,] {
            { 'B', 'Q', 'I', 'F', 'T' },
            { 'G', 'P', 'O', 'C', 'H' },
            { 'W', 'N', 'A', 'L', 'Y' },
            { 'K', 'D', 'X', 'U', 'V' },
            { 'Z', 'S', 'E', 'M', 'R' }
        };

        const string plaintext = "FLEEATONCE";
        const string ciphertext = "UAEOLWRINS";

        Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key), true);
        Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key), true);
    }
    */
	[TestMethod]
	public void Vigenère()
	{
		var cipher = new VigenèreCipher();

		const string key = "Lemon";

		const string plaintext = "Attack at dawn";
		const string ciphertext = "Lxfopv ef rnhr";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key.ToCharArray()));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key.ToCharArray()));
	}

	[TestMethod]
	public void Bacon()
	{
		var cipher = new BaconCipher();

		const string plaintext = "HELLO WORLD";
		const string ciphertext = "AABBBAABAAABABAABABAABBAB BABAAABBABBAAAAABABAAAABB";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext), true);
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext), true);
	}

	[TestMethod]
	public void Rot47()
	{
		var cipher = new Rot47Cipher();

		const string plaintext = "My string!";
		const string ciphertext = "|J DEC:?8P";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
	}

    [TestMethod]
    public void Autokey()
    {
        var cipher = new AutokeyCipher();

        const string key = "QUEENLY";

        const string plaintext = "Attack AT DAWN";
        const string ciphertext = "Qnxepv YT WTWP";

        Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key.ToCharArray()));
        Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key.ToCharArray()));
    }

	/*
    [TestMethod]
    public void Sandorf()
    {
        var cipher = new SandorfCipher();

        bool[,] key = {
            { false, false, false, false, false, false, },
            { true, false, false, true, false, false },
            { false, false, true, false, false, false },
            { true, false, false, false, false, true },
            { false, true, false, true, false, false },
            { true, false, false, false, true, false }
        };

        const string plaintext = "Real Programmers use Fortran. Quiche Eaters use Pascal.";
        const string ciphertext = "u#l# #as#r##ec##st##aa##EP# ## #e#s.geuhoc raPir sutrlrQae oemFR .meanrs";

        Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
        Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
    }
	*/
    [TestMethod]
    public void TwoSquare()
    {
        var cipher = new TwoSquareCipher();
        var key = (
            PolybiusSquare.CreateFromKeyword("EXAMPLE", WellKnownAlphabets.EnglishWithoutQ),
            PolybiusSquare.CreateFromKeyword("KEYWORD", WellKnownAlphabets.EnglishWithoutQ)
        );

        const string plaintext = "Help me Obiwan Kenobi!";
        const string ciphertext = "Hedl xw Sdjyan Hotkdg!";

        Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
        Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
    }
	
    [TestMethod]
    public void FourSquare()
    {
        var cipher = new FourSquareCipher();
        var key = (
            PolybiusSquare.CreateFromAlphabet(WellKnownAlphabets.EnglishWithoutQ),
            PolybiusSquare.CreateFromKeyword("EXAMPLE", WellKnownAlphabets.EnglishWithoutQ), 
			PolybiusSquare.CreateFromKeyword("KEYWORD", WellKnownAlphabets.EnglishWithoutQ),
			PolybiusSquare.CreateFromAlphabet(WellKnownAlphabets.EnglishWithoutQ)
        );

        const string plaintext = "Help me Obiwan Kenobi";
        const string ciphertext = "Fygm ky Hobxmf Kkkimd";

        Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
        Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
    }

	[TestMethod]
	public void Trithemius()
	{
		var cipher = new TrithemiusCipher();

		const string plaintext = "The quick brown fox jumps over the lazy dog.";
		const string ciphertext = "Tig tynir jayhz scm zleim jrbp shf nddd jvo.";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
	}

	[TestMethod]
	public void Gronsfeld()
	{
		var cipher = new GronsfeldCipher();

		int[] key = { 3, 2, 6 };

		const string plaintext = "Geheimnis";
		const string ciphertext = "Jgnhksqky";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
	}

	[TestMethod]
	public void VariantBeaufort()
	{
		var cipher = new VariantBeaufortCipher();

		const string key = "CODE";

		const string plaintext = "The quick brown fox jumps over the lazy dog.";
		const string ciphertext = "Rtb msuzg zdlsl rlt hgjlq asap fea jmwu bad.";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key.ToCharArray()));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key.ToCharArray()));
	}

	[TestMethod]
	public void Gudhayojya()
	{
		var cipher = new GudhayojyaCipher();

		const string key = "dis";

		const string plaintext = "will visit you tonight.";
		const string ciphertext = "diswill disvisit disyou distonight.";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key.ToCharArray()));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key.ToCharArray()));
	}

	[TestMethod]
	public void Beaufort()
	{
		var cipher = new BeaufortCipher();

		const string key = "m";

		const string plaintext = "d J";
		const string ciphertext = "j D";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key.ToCharArray()));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key.ToCharArray()));
	}

	[TestMethod]
	public void Base64()
	{
		var cipher = new Base64Encoder();

		const string plaintext = "The quick brown fox jumps over the lazy dog.";
		const string ciphertext = "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZy4=";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
	}

	[TestMethod]
	public void Hex()
	{
		var cipher = new HexEncoder();

		const string plaintext = "The quick brown fox jumps over the lazy dog.";
		const string ciphertext = "54686520717569636b2062726f776e20666f78206a756d7073206f76657220746865206c617a7920646f672e";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext), ignoreCase: true);
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
	}

	[TestMethod]
	public void Binary()
	{
		var cipher = new AsciiBinaryBase();

		const string plaintext = "The quick brown fox jumps over the lazy dog.";
		const string ciphertext = "0101010001101000011001010010000001110001011101010110100101100011011010110010000001100010011100100110111101110111011011100010000001100110011011110111100000100000011010100111010101101101011100000111001100100000011011110111011001100101011100100010000001110100011010000110010100100000011011000110000101111010011110010010000001100100011011110110011100101110";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
	}

	[TestMethod]
	public void Playfair()
	{
		var cipher = new PlayfairCipher();

		const string plaintext = "Hide the GOLD IN THE TREXE STUMP.";
		const string ciphertext = "Bmod zbx DNAB EK UDM UIXMM OUVIF.";
		var square = PolybiusSquare.CreateFromCharacters("PLAYFIREXMBCDGHKNOQSTUVWZ");

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, square));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, square));
	}

	[TestMethod]
	public void Malespin()
	{
		var cipher = new MalespinSlang();

		const string plaintext = "Malespin";
		const string ciphertext = "Pelasmon";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
	}

	[TestMethod]
	public void Wolfenbütteler()
	{
		var cipher = new WolfenbüttelerCipher();

		const string plaintext = "Beispielklartext";
		const string ciphertext = "Bkdspdklelmrokxo";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
	}

	[TestMethod]
	public void OneTimePad()
	{
		var cipher = new OneTimePadCipher();

		const string plaintext = "Hello";
		const string ciphertext = "Eqnvz";
		const string key = "XMCKL";

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key.ToCharArray()));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key.ToCharArray()));
	}

	[TestMethod]
	public void Scytale()
	{
		var cipher = new ScytaleCipher();

		const string plaintext = "Iamhurtverybadlyhelp";
		const string ciphertext = "Iryyatbhmvaehedlurlp";
		const int key = 4;

		Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
		Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
	}
}
