using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests
{
    [TestClass]
    public class WordlistSpeculativePlaintextRankerTests
    {
        [TestMethod]
        public void Rank_Full()
        {
            var ranker = new WordlistSpeculativePlaintextRanker(new[] {
                "the",
                "answer",
                "is",
                "wordlist",
            });
            var result = ranker.Classify("theansweriswordlist");
            
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Rank()
        {
            var ranker = new WordlistSpeculativePlaintextRanker(new[] {
                "the",
                "answer",
                "is",
                "wordlist",
            });
            var result = ranker.Classify("the answer is: wordlist.");

            Assert.AreEqual(19 / 24D, result);
        }

        [TestMethod]
        public void Rank_CaseInsensitive()
        {
            var ranker = new WordlistSpeculativePlaintextRanker(new[] {
                "the",
                "answeR",
                "is",
                "Wordlist",
            });
            var result = ranker.Classify("The answer Is: wordList.");

            Assert.AreEqual(19 / 24D, result);
        }

        [TestMethod]
        public void Rank_Opposite()
        {
            var ranker = new WordlistSpeculativePlaintextRanker(new[] {
                "the",
                "answer",
                "is",
                "wordlist",
            });
            var result = ranker.Classify("does not match.");

            Assert.AreEqual(0, result);
        }
    }
}
