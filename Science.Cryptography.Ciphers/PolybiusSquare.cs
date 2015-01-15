using System;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Contains methods for creating and manipulating Polybius Squares.
    /// </summary>
    public static class PolybiusSquare
    {
        public static char[,] Regular
        {
            get
            {
                return new char[,] {
                    { 'A', 'B', 'C', 'D', 'E' },
                    { 'F', 'G', 'H', 'I', 'K' },
                    { 'L', 'M', 'N', 'O', 'P' },
                    { 'Q', 'R', 'S', 'T', 'U' },
                    { 'V', 'W', 'X', 'Y', 'Z' }
                };
            }
        }

        public static char[,] RegularWithJ
        {
            get
            {
                return new char[,] {
                    { 'A', 'B', 'C', 'D', 'E' },
                    { 'F', 'G', 'H', 'J', 'K' },
                    { 'L', 'M', 'N', 'O', 'P' },
                    { 'Q', 'R', 'S', 'T', 'U' },
                    { 'V', 'W', 'X', 'Y', 'Z' }
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="alphabet"></param>
        /// <returns></returns>
        public static char[,] CreateFromKeyword(string keyword, string alphabet)
        {
            return CreateFromCharArray(
                keyword.ToCharArray().Select(Char.ToUpper)
                .Concat(alphabet.ToCharArray())
                .Distinct()
                .ToArray()
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static char[,] CreateFromKeyword(string keyword)
        {
            return CreateFromKeyword(keyword, Charsets.EnglishWithoutJ);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static char[,] CreateFromCharArray(char[] source)
        {
            int size = (int)Math.Sqrt(source.Length);

            char[,] result = new char[size, size];

            for (int i = 0; i < source.Length; i++)
                result[i % size, i / size] = source[i];

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static char[,] CreateFromString(string source)
        {
            int size = (int)Math.Sqrt(source.Length);

            char[,] result = new char[size, size];

            for (int i = 0; i < source.Length; i++)
                result[i % size, i / size] = source[i];

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="polybiusSquare"></param>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static Tuple<int, int> FindOffsets(char[,] polybiusSquare, char ch)
        {
            int width = polybiusSquare.GetLength(0), height = polybiusSquare.GetLength(1);

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (polybiusSquare[x, y] == ch)
                    return new Tuple<int, int>(x, y);

            return null;
        }
    }
}
