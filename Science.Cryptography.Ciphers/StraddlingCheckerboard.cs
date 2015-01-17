using System;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Contains methods for creating a manipulating straddling checkerboards.
    /// </summary>
    public static class StraddlingCheckerboard
    {
        private const int Width = 10, Height = 3;

        /// <summary>
        /// Creates a straddling checkerboard from a char array.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static char[,] CreateFromCharArray(char[] source)
        {
            char[,] result = new char[Width, Height];

            for (int i = 0; i < source.Length; i++)
                result[i % Width, i / Height] = source[i];

            return result;
        }

        /// <summary>
        /// Creates a straddling checkerboard from a string.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static char[,] CreateFromString(string source)
        {
            char[,] result = new char[Width, Height];

            for (int i = 0; i < source.Length; i++)
                result[i % Width, i / Height] = source[i];

            return result;
        }

        /// <summary>
        /// Creates a straddling checkerboard from a keyword, based on a custom charset.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static char[,] CreateFromKeyword(string keyword, string charset)
        {
            return CreateFromCharArray(
                keyword.AsEnumerable().Select(Char.ToUpper)
                    .Concat(charset.AsEnumerable())
                    .Distinct()
                    .ToArray()
            );
        }

        /// <summary>
        /// Creates a straddling checkerboard from a keyword, based on the default charset.
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static char[,] CreateFromKeyword(string keyword)
        {
            return CreateFromKeyword(keyword, Charsets.English);
        }

        public static int FindOffsets(char[,] straddlingCheckerboard, char ch)
        {
            int width = straddlingCheckerboard.GetLength(0),
                height = straddlingCheckerboard.GetLength(1);

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (straddlingCheckerboard[x, y] == ch)
                    return y * 10 + x;

            return -1;
        }
    }
}
