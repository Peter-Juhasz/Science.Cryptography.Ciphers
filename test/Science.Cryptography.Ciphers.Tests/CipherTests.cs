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

            const string plaintext = "The quick brown fox jumps over the lazy dog.";
            const string ciphertext = "Wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj.";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, WellKnownShiftCipherKeys.Caesar));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, WellKnownShiftCipherKeys.Caesar));
        }

        [TestMethod]
        public void Rot13()
        {
            ShiftCipher cipher = new ShiftCipher();

            const string plaintext = "Why did the chicken cross the road?";
            const string ciphertext = "Jul qvq gur puvpxra pebff gur ebnq?";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, WellKnownShiftCipherKeys.Rot13));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, WellKnownShiftCipherKeys.Rot13));
        }

        [TestMethod]
        public void TapCode()
        {
            TapCode cipher = new TapCode();

            const string plaintext = "WATER";
            const string ciphertext = "..... .. . . .... .... . ..... .... ..";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext), true);
        }

        [TestMethod]
        public void MorseCode()
        {
            MorseCode cipher = new MorseCode();

            const string plaintext = "MORSE CODE";
            const string ciphertext = "-- --- .-. ... .   -.-. --- -.. .";

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

        [TestMethod]
        public void Atbash()
        {
            AtbashCipher cipher = new AtbashCipher();

            const string plaintext = "Abcdefghijklmnopqrstuvwxyz";
            const string ciphertext = "Zyxwvutsrqponmlkjihgfedcba";

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

            const string plaintext = "FLEEATONCE";
            const string ciphertext = "UAEOLWRINS";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key), true);
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key), true);
        }

        [TestMethod]
        public void Vigenère()
        {
            VigenèreCipher cipher = new VigenèreCipher();

            const string key = "Lemon";

            const string plaintext = "Attack at dawn";
            const string ciphertext = "Lxfopv ef rnhr";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }

        [TestMethod]
        public void Bacon()
        {
            BaconCipher cipher = new BaconCipher();

            const string plaintext = "HELLO WORLD";
            const string ciphertext = "AABBBAABAAABABAABABAABBAB BABAAABBABBAAAAABABAAAABB";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext), true);
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext), true);
        }

        [TestMethod]
        public void Rot47()
        {
            Rot47Cipher cipher = new Rot47Cipher();

            const string plaintext = "My string!";
            const string ciphertext = "|J DEC:?8P";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
        }

        [TestMethod]
        public void Autokey()
        {
            AutokeyCipher cipher = new AutokeyCipher();

            const string key = "QUEENLY";

            const string plaintext = "Attack AT DAWN";
            const string ciphertext = "Qnxepv YT WTWP";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }

        [TestMethod]
        public void Sandorf()
        {
            SandorfCipher cipher = new SandorfCipher();

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

        [TestMethod]
        public void TwoSquare()
        {
            TwoSquareCipher cipher = new TwoSquareCipher();

            char[][,] key = {
                PolybiusSquare.CreateFromKeyword("EXAMPLE", Charsets.EnglishWithoutQ),
                PolybiusSquare.CreateFromKeyword("KEYWORD", Charsets.EnglishWithoutQ)
            };

            const string plaintext = "Help me Obiwan Kenobi";
            const string ciphertext = "Hedl xw Sdjyan Hotkdg";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }

        [TestMethod]
        public void FourSquare()
        {
            FourSquareCipher cipher = new FourSquareCipher();

            char[,][,] key = {
                { PolybiusSquare.CreateFromString(Charsets.EnglishWithoutQ), PolybiusSquare.CreateFromKeyword("KEYWORD", Charsets.EnglishWithoutQ) },
                { PolybiusSquare.CreateFromKeyword("EXAMPLE", Charsets.EnglishWithoutQ), PolybiusSquare.CreateFromString(Charsets.EnglishWithoutQ) }
            };

            const string plaintext = "Help me Obiwan Kenobi";
            const string ciphertext = "Fygm ky Hobxmf Kkkimd";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }

        [TestMethod]
        public void Trithemius()
        {
            ICipher cipher = new TrithemiusCipher();
            
            const string plaintext = "The quick brown fox jumps over the lazy dog.";
            const string ciphertext = "Tig tynir jayhz scm zleim jrbp shf nddd jvo.";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext));
        }

        [TestMethod]
        public void Gronsfeld()
        {
            GronsfeldCipher cipher = new GronsfeldCipher();

            int[] key = { 3, 2, 6 };

            const string plaintext = "Geheimnis";
            const string ciphertext = "Jgnhksqky";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }

        [TestMethod]
        public void VariantBeaufort()
        {
            VariantBeaufortCipher cipher = new VariantBeaufortCipher();

            const string key = "CODE";

            const string plaintext = "The quick brown fox jumps over the lazy dog.";
            const string ciphertext = "Rtb msuzg zdlsl rlt hgjlq asap fea jmwu bad.";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }

        [TestMethod]
        public void Gudhayojya()
        {
            GudhayojyaCipher cipher = new GudhayojyaCipher();

            const string key = "dis";

            const string plaintext = "will visit you tonight.";
            const string ciphertext = "diswill disvisit disyou distonight.";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }

        [TestMethod]
        public void Beaufort()
        {
            BeaufortCipher cipher = new BeaufortCipher();

            const string key = "m";

            const string plaintext = "d J";
            const string ciphertext = "j D";

            Assert.AreEqual(ciphertext, cipher.Encrypt(plaintext, key));
            Assert.AreEqual(plaintext, cipher.Decrypt(ciphertext, key));
        }
    }
}
