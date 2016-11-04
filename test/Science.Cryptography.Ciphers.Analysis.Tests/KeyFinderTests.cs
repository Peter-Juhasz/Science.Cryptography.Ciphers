using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests
{
    [TestClass]
    public class KeyFinderTests
    {
        [TestMethod]
        public void FindBest_Shift_Substring()
        {
            string ciphertext = "Znk ykixkz oy Igkygx.";

            var result = KeyFinder.FindBest(
                ciphertext,
                new ShiftCipher(),
                new ShiftKeySpaceSource(),
                new SubstringSpeculativePlaintextRanker("secret")
            );

            Assert.AreEqual(1.0, result.Rank);
            Assert.AreEqual("The secret is Caesar.", result.SpeculativePlaintext);
        }

        [TestMethod]
        public void FindBest_NoMatch()
        {
            string ciphertext = "Znk ykixkz oy Igkygx.";

            var result = KeyFinder.FindBest(
                ciphertext,
                new ShiftCipher(),
                new ShiftKeySpaceSource(),
                new SubstringSpeculativePlaintextRanker("answer")
            );

            Assert.IsNull(result);
        }
    }
}
