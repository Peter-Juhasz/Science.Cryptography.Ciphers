using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Four Square cipher.
    /// </summary>
    [Export("Four-square", typeof(IKeyedCipher<>))]
    public class FourSquareCipher : IKeyedCipher<char[,][,]>
    {
        public string Encrypt(string plaintext, char[,][,] key)
        {
            char[] result = new char[plaintext.Length];
            
            for (int i = 0; i < plaintext.Length - 1; i++)
            {
                if (Char.IsLetter(plaintext[i]) && Char.IsLetter(plaintext[i + 1]))
                {
                    Tuple<int, int> firstPosition = PolybiusSquare.FindOffsets(key[0, 0], Char.ToUpper(plaintext[i])),
                        secondPosition = PolybiusSquare.FindOffsets(key[1, 1], Char.ToUpper(plaintext[i + 1]));

                    if (firstPosition == null || secondPosition == null)
                    {
                        result[i] = plaintext[i];
                        result[i + 1] = plaintext[i + 1];

                        i++;
                        continue;
                    }

                    result[i] = key[1, 0][secondPosition.Item1, firstPosition.Item2].ToSameCaseAs(plaintext[i]);
                    result[i + 1] = key[0, 1][firstPosition.Item1, secondPosition.Item2].ToSameCaseAs(plaintext[i + 1]);
                    i++;
                }
                else
                    result[i] = plaintext[i];
            }

            return new String(result);
        }

        public string Decrypt(string ciphertext, char[,][,] key)
        {
            char[] result = new char[ciphertext.Length];

            for (int i = 0; i < ciphertext.Length - 1; i++)
            {
                if (Char.IsLetter(ciphertext[i]) && Char.IsLetter(ciphertext[i + 1]))
                {
                    Tuple<int, int> firstPosition = PolybiusSquare.FindOffsets(key[1, 0], Char.ToUpper(ciphertext[i])),
                        secondPosition = PolybiusSquare.FindOffsets(key[0, 1], Char.ToUpper(ciphertext[i + 1]));

                    if (firstPosition == null || secondPosition == null)
                    {
                        result[i] = ciphertext[i];
                        result[i + 1] = ciphertext[i + 1];

                        i++;
                        continue;
                    }

                    result[i] = key[0, 0][secondPosition.Item1, firstPosition.Item2].ToSameCaseAs(ciphertext[i]);
                    result[i + 1] = key[1, 1][firstPosition.Item1, secondPosition.Item2].ToSameCaseAs(ciphertext[i + 1]);
                    i++;
                }
                else
                    result[i] = ciphertext[i];
            }

            return new String(result);
        }
    }
}
