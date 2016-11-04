using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests
{
    [TestClass]
    public class CharsetSpeculativePlaintextRankerTests
    {
        [TestMethod]
        public void Rank_Full()
        {
            var ranker = new CharsetSpeculativePlaintextRanker(new[] { 'A', 'B', 'C' });
            var result = ranker.Classify("ABCCBA");
            
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Rank()
        {
            var ranker = new CharsetSpeculativePlaintextRanker(new[] { 'A', 'B', 'C' });
            var result = ranker.Classify("ADBCEF");

            Assert.AreEqual(0.5, result);
        }

        [TestMethod]
        public void Rank_Opposite()
        {
            var ranker = new CharsetSpeculativePlaintextRanker(new[] { 'A', 'B', 'C' });
            var result = ranker.Classify("XYZ");

            Assert.AreEqual(0, result);
        }
    }
}
