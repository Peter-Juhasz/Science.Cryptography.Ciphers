﻿using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Two Square cipher.
    /// </summary>
    [Export("Two-square", typeof(IKeyedCipher<>))]
    public class TwoSquareCipher : ReciprocalKeyedCipher<char[][,]>
    {
        protected override string Crypt(string text, char[][,] key)
        {
            char[] result = new char[text.Length];

            for (int i = 0; i < text.Length - 1; i++)
            {
                if (Char.IsLetter(text[i]) && Char.IsLetter(text[i + 1]))
                {
                    Tuple<int, int> firstPosition = PolybiusSquare.FindOffsets(key[0], Char.ToUpper(text[i])),
                        secondPosition = PolybiusSquare.FindOffsets(key[1], Char.ToUpper(text[i + 1]));

                    if (firstPosition == null || secondPosition == null)
                    {
                        result[i] = text[i];
                        result[i + 1] = text[i + 1];

                        i++;
                        continue;
                    }

                    result[i] = key[0][secondPosition.Item1, firstPosition.Item2].ToSameCaseAs(text[i]);
                    result[i + 1] = key[1][firstPosition.Item1, secondPosition.Item2].ToSameCaseAs(text[i + 1]);
                    i++;
                }
                else
                    result[i] = text[i];
            }

            return new String(result);
        }
    }
}
