using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the ROT-47 cipher.
    /// </summary>
    [Export("ROT-47", typeof(ICipher))]
    public class Rot47Cipher : ICipher
    {
        public string Encrypt(string plaintext)
        {
            return Crypt(plaintext);
        }

        public string Decrypt(string ciphertext)
        {
            return Crypt(ciphertext);
        }


        protected static string Crypt(string text)
        {
            char[] result = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    result[i] = ' ';
                    continue;
                }

                int ascii = text[i] + 47;

                if (ascii > 126)
                    ascii -= 94;
                if (ascii < 33)
                    ascii += 94;

                result[i] = (char)ascii;
            }

            return new String(result);
        }
    }
}
