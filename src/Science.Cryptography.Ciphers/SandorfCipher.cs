using System;
using System.Composition;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents Count Mathias Sandorf's (hero of Jules Verne) cipher.
    /// </summary>
    [Export("Sandorf", typeof(IKeyedCipher<>))]
    public class SandorfCipher : IKeyedCipher<bool[,]>
    {
        private const char PaddingChar = '#';


        public string Encrypt(string plaintext, bool[,] key)
        {
            int size = key.GetLength(0);

            if (size != key.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(key), "Only square-sized keys are supported.");

            int sizeSquared = size * size;

            // pad to a full square
            if (plaintext.Length % sizeSquared != 0)
                plaintext = plaintext.PadRight(plaintext.Length + (sizeSquared - plaintext.Length % sizeSquared), PaddingChar);

            // reverse
            plaintext = new String(plaintext.AsEnumerable().Reverse().ToArray());

            // build code groups
            char[][,] groups = new char[plaintext.Length / sizeSquared][,];
            int currentGroupIndex = 0;

            foreach (string substring in plaintext.Split(sizeSquared))
            {
                char[,] group = new char[size, size];
                int offset = 0;

                // rotate the key and fill up with chars
                for (int r = 0; r < 4; r++)
                {
                    for (int y = 0; y < size; y++)
                    for (int x = 0; x < size; x++)
                        if (key[x, y])
                            group[x, y] = substring[offset++];

                    key = key.RotateClockwise();
                }

                groups[currentGroupIndex++] = group;
            }

            // concatenate code groups to a string
            char[] result = new char[plaintext.Length];
            int i = 0;

            foreach (char[,] group in groups)
            {
                for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    result[i++] = group[x, y];
            }

            return new String(result);
        }

        public string Decrypt(string ciphertext, bool[,] key)
        {
            int size = key.GetLength(0);

            if (size != key.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(key), "Only square-sized keys are supported.");

            int sizeSquared = size * size;

            // reconstruct code groups
            char[][,] groups = new char[ciphertext.Length / sizeSquared][,];
            int currentGroupIndex = 0;

            foreach (string substring in ciphertext.Split(sizeSquared))
            {
                char[,] group = new char[size, size];
                int offset = 0;

                for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    group[x, y] = substring[offset++];

                groups[currentGroupIndex++] = group;
            }

            // reconstruct strings using the key
            char[] result = new char[ciphertext.Length];
            key = key.RotateCounterClockwise();
            currentGroupIndex = 0;

            foreach (char[,] group in groups)
            {
                int i = 0;

                for (int r = 0; r < 4; r++)
                {
                    for (int y = size - 1; y >= 0; y--)
                    for (int x = size - 1; x >= 0; x--)
                        if (key[x, y])
                            result[(groups.Length - currentGroupIndex - 1) * sizeSquared + i++] = group[x, y];

                    key = key.RotateCounterClockwise();
                }

                currentGroupIndex++;
            }

            return new String(result).TrimEnd(PaddingChar);
        }
    }
}
