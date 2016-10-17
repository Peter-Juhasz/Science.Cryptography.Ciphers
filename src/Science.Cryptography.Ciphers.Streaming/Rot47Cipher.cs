using System.Collections.Generic;
using System.Composition;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents the ROT-47 cipher.
    /// </summary>
    [Export("ROT-47", typeof(ICipher))]
    public class Rot47Cipher : ICipher
    {
        public IEnumerable<char> Encrypt(IEnumerable<char> plaintext)
        {
            return Crypt(plaintext);
        }

        public IEnumerable<char> Decrypt(IEnumerable<char> ciphertext)
        {
            return Crypt(ciphertext);
        }


        protected static IEnumerable<char> Crypt(IEnumerable<char> text)
        {
            foreach (char c in text)
            {
                if (c == ' ')
                    yield return c;

                int ascii = c + 47;

                if (ascii > 126)
                    ascii -= 94;
                if (ascii < 33)
                    ascii += 94;

                yield return (char)ascii;
            }
        }
    }
}
