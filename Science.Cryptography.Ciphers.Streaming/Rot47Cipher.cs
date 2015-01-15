using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents the ROT-47 cipher.
    /// </summary>
    public class Rot47Cipher : ICipher
    {
        protected IEnumerable<char> Crypt(IEnumerable<char> text)
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

        public IEnumerable<char> Encrypt(IEnumerable<char> plaintext)
        {
            return this.Crypt(plaintext);
        }

        public IEnumerable<char> Decrypt(IEnumerable<char> ciphertext)
        {
            return this.Crypt(ciphertext);
        }
    }
}
