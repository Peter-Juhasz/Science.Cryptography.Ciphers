using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests
{
    [TestClass]
    public class CipherTests
    {
        [TestMethod]
        public void Caesar()
        {
            ShiftCipher cipher = new ShiftCipher();

            string plaintext = "The quick brown fox jumps over the lazy dog.";
            string ciphertext = "Wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj.";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, WellKnownShiftCipherKeys.Caesar));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, WellKnownShiftCipherKeys.Caesar));
        }

        [TestMethod]
        public void Rot13()
        {
            ShiftCipher cipher = new ShiftCipher();

            string plaintext = "Why did the chicken cross the road?";
            string ciphertext = "Jul qvq gur puvpxra pebff gur ebnq?";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, WellKnownShiftCipherKeys.Rot13));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, WellKnownShiftCipherKeys.Rot13));
        }

        [TestMethod]
        public void TapCode()
        {
            TapCode cipher = new TapCode();

            string plaintext = "WATER";
            string ciphertext = "..... .. . . .... .... . ..... .... ..";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext), true);
        }

        [TestMethod]
        public void MorseCode()
        {
            MorseCode cipher = new MorseCode();

            string plaintext = "MORSE CODE";
            string ciphertext = "-- --- .-. ... .   -.-. --- -.. .";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext), true);
        }

        [TestMethod]
        public void Atbash()
        {
            AtbashCipher cipher = new AtbashCipher();

            string plaintext = "Abcdefghijklmnopqrstuvwxyz";
            string ciphertext = "Zyxwvutsrqponmlkjihgfedcba";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
        }

        [TestMethod]
        public void Bifid()
        {
            BifidCipher cipher = new BifidCipher();

            char[,] key = new char[,] {
                { 'B', 'Q', 'I', 'F', 'T' },
                { 'G', 'P', 'O', 'C', 'H' },
                { 'W', 'N', 'A', 'L', 'Y' },
                { 'K', 'D', 'X', 'U', 'V' },
                { 'Z', 'S', 'E', 'M', 'R' }
            };

            string plaintext = "FLEEATONCE";
            string ciphertext = "UAEOLWRINS";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key), true);
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key), true);
        }

        [TestMethod]
        public void Vigenère()
        {
            VigenèreCipher cipher = new VigenèreCipher();

            string plaintext = "Attack at dawn";
            string key = "Lemon";
            string ciphertext = "Lxfopv ef rnhr";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }

        [TestMethod]
        public void Bacon()
        {
            BaconCipher cipher = new BaconCipher();

            string plaintext = "HELLO WORLD";
            string ciphertext = "AABBBAABAAABABAABABAABBAB BABAAABBABBAAAAABABAAAABB";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext), true);
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext), true);
        }

        [TestMethod]
        public void Rot47()
        {
            Rot47Cipher cipher = new Rot47Cipher();

            string plaintext = "My string!";
            string ciphertext = "|J DEC:?8P";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
        }

        [TestMethod]
        public void Autokey()
        {
            AutokeyCipher cipher = new AutokeyCipher();

            string key = "QUEENLY";

            string plaintext = "Attack AT DAWN";
            string ciphertext = "Qnxepv YT WTWP";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }

        [TestMethod]
        public void Sandorf()
        {
            SandorfCipher cipher = new SandorfCipher();

            bool[,] key = new bool[,] {
                { false, false, false, false, false, false, },
                { true, false, false, true, false, false },
                { false, false, true, false, false, false },
                { true, false, false, false, false, true },
                { false, true, false, true, false, false },
                { true, false, false, false, true, false }
            };

            string plaintext = "Real Programmers use Fortran. Quiche Eaters use Pascal.";
            string ciphertext = "u#l# #as#r##ec##st##aa##EP# ## #e#s.geuhoc raPir sutrlrQae oemFR .meanrs";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }

        [TestMethod]
        public void TwoSquare()
        {
            TwoSquareCipher cipher = new TwoSquareCipher();

            char[][,] key = new char[2][,] {
                PolybiusSquare.CreateFromKeyword("EXAMPLE", Charsets.EnglishWithoutQ),
                PolybiusSquare.CreateFromKeyword("KEYWORD", Charsets.EnglishWithoutQ)
            };

            string plaintext = "Help me Obiwan Kenobi";
            string ciphertext = "Hedl xw Sdjyan Hotkdg";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }

        [TestMethod]
        public void FourSquare()
        {
            FourSquareCipher cipher = new FourSquareCipher();

            char[,][,] key = new char[2, 2][,] {
                { PolybiusSquare.CreateFromString(Charsets.EnglishWithoutQ), PolybiusSquare.CreateFromKeyword("KEYWORD", Charsets.EnglishWithoutQ) },
                { PolybiusSquare.CreateFromKeyword("EXAMPLE", Charsets.EnglishWithoutQ), PolybiusSquare.CreateFromString(Charsets.EnglishWithoutQ) }
            };

            string plaintext = "Help me Obiwan Kenobi";
            string ciphertext = "Fygm ky Hobxmf Kkkimd";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }
    }
}
