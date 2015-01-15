using System;
using System.Linq;
using System.Text;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Bifid cipher.
    /// </summary>
    public class BifidCipher : IKeyedCipher<char[,]>
    {
        public string Encrypt(string plaintext, char[,] key)
        {
            // F L E E A T O N C E
            // 4 4 3 3 3 5 3 2 4 3
            // 1 3 5 5 3 1 2 3 2 5
            int[] rowIndexes = new int[plaintext.Length],
                columnIndexes = new int[plaintext.Length];

            for (int i = 0; i < plaintext.Length; i++)
            {
                Tuple<int, int> offsets = PolybiusSquare.FindOffsets(key, plaintext[i]);
                columnIndexes[i] = offsets.Item1;
                rowIndexes[i] = offsets.Item2;
            }

            StringBuilder result = new StringBuilder(plaintext.Length);

            // 4 4 3 3 3 5 3 2 4 3 1 3 5 5 3 1 2 3 2 5
            int[] concatenated = rowIndexes.Concat(columnIndexes).ToArray();

            // 44 33 35 32 43 13 55 31 23 25
            // U  A  E  O  L  W  R  I  N  S
            for (int i = 0; i < concatenated.Length; i += 2)
                result.Append(key[concatenated[i + 1], concatenated[i]]);

            return result.ToString();
        }

        public string Decrypt(string ciphertext, char[,] key)
        {
            int[] rowIndexes = new int[ciphertext.Length],
                columnIndexes = new int[ciphertext.Length];

            for (int i = 0; i < ciphertext.Length; i++)
            {
                Tuple<int, int> offsets = PolybiusSquare.FindOffsets(key, ciphertext[i]);
                columnIndexes[i] = offsets.Item1;
                rowIndexes[i] = offsets.Item2;
            }

            StringBuilder result = new StringBuilder(ciphertext.Length);

            int[] concatenated = new int[ciphertext.Length * 2];

            for (int i = 0; i < ciphertext.Length; i++)
            {
                concatenated[2 * i] = rowIndexes[i];
                concatenated[2 * i + 1] = columnIndexes[i];
            }

            for (int i = 0; i < ciphertext.Length; i++)
                result.Append(key[concatenated[i + ciphertext.Length], concatenated[i]]);

            return result.ToString();
        }
    }
}
