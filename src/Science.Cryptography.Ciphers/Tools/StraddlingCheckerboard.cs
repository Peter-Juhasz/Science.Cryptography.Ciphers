using System;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Contains methods for creating a manipulating straddling checkerboards.
    /// </summary>
    public class StraddlingCheckerboard
    {
        private StraddlingCheckerboard(char[,] buffer)
        {
            _buffer = buffer;
        }

        private const int Width = 10, Height = 3;

        private readonly char[,] _buffer;

        /// <summary>
        /// Gets a <see cref="char"/> at a specified position.
        /// </summary>
        /// <param name="row">The row of the checkerboard.</param>
        /// <param name="column">The column of the checkerboard.</param>
        /// <returns></returns>
        public char this[int row, int column]
        {
            get { return _buffer[column, row]; }
        }

        /// <summary>
        /// Creates a straddling checkerboard from a <see cref="char[]"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static StraddlingCheckerboard CreateFromCharArray(char[] source)
        {
            return new StraddlingCheckerboard(CreateBufferFromCharArray(source));
        }

        /// <summary>
        /// Creates a straddling checkerboard from a <see cref="string"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static StraddlingCheckerboard CreateFromString(string source)
        {
            return new StraddlingCheckerboard(CreateBufferFromString(source));
        }

        /// <summary>
        /// Creates a straddling checkerboard from a keyword, based on a custom charset.
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static StraddlingCheckerboard CreateFromKeyword(string keyword, string charset = Charsets.English)
        {
            return new StraddlingCheckerboard(CreateBufferFromKeyword(keyword, charset));
        }
        

        internal static char[,] CreateBufferFromCharArray(char[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            char[,] result = new char[Width, Height];
            
            for (int i = 0; i < source.Length; i++)
                result[i % Width, i / Height] = source[i];

            return result;
        }

        internal static char[,] CreateBufferFromString(string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            char[,] result = new char[Width, Height];

            for (int i = 0; i < source.Length; i++)
                result[i % Width, i / Height] = source[i];

            return result;
        }

        internal static char[,] CreateBufferFromKeyword(string keyword, string charset = Charsets.English)
        {
            if (keyword == null)
                throw new ArgumentNullException(nameof(keyword));

            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            return CreateBufferFromCharArray(
                keyword.Select(Char.ToUpper)
                    .Concat(charset)
                    .Distinct()
                    .ToArray()
            );
        }


        /// <summary>
        /// Returns a copy of the buffer behind the <see cref="StraddlingCheckerboard"/>.
        /// </summary>
        /// <returns></returns>
        public char[,] ToCharArray()
        {
            return (char[,])_buffer.Clone();
        }
    }
}
