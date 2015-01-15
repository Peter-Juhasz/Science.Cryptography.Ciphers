using System;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the ROT-47 cipher.
    /// </summary>
    public class Rot47Cipher : ICipher
    {
        protected string Crypt(string text)
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

        public string Encrypt(string plaintext)
        {
            return this.Crypt(plaintext);
        }

        public string Decrypt(string ciphertext)
        {
            return this.Crypt(ciphertext);
        }
    }
}
