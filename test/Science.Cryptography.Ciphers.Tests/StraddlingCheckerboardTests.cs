using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Tests
{
    [TestClass]
    public class StraddlingCheckerboardTests
    {
        [TestMethod]
        public void CreateFromString()
        {
            StraddlingCheckerboard sc = StraddlingCheckerboard.CreateFromString(Charsets.English);
            Assert.AreEqual('A', sc[0, 0]);
            Assert.AreEqual('B', sc[0, 1]);
        }
        
        [TestMethod]
        public void ToCharArray()
        {
            StraddlingCheckerboard sc = StraddlingCheckerboard.CreateFromString(Charsets.English);
            char[,] buffer = sc.ToCharArray();

            Assert.AreEqual(sc[1, 2], buffer[1, 2]);
            buffer[1, 2] = 'X';
            Assert.AreEqual(sc[1, 2], buffer[1, 2]);
        }
    }
}
