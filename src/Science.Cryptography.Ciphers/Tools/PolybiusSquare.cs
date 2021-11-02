using System;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Contains methods for creating and manipulating Polybius Squares.
    /// </summary>
    public struct PolybiusSquare
    {
        public PolybiusSquare(char[,] data)
        {
            _data = data;
        }

        private readonly char[,] _data;

        public char this[int column, int row] => _data[column, row];

        public char this[(int column, int row) position] => _data[position.column, position.row];

        public (int x, int y) FindOffsets(char ch) => FindOffsets(_data, ch);

        public bool TryFindOffsets(char ch, out (int x, int y) position) => TryFindOffsets(_data, ch, out position);


        public static implicit operator PolybiusSquare(char[,] s) => new(s);


        public static readonly PolybiusSquare RegularWithoutI = CreateFromString("ABCDEFGHJKLMNOPQRSTUVWXYZ");

        public static readonly PolybiusSquare RegularWithoutK = CreateFromString("ABCDEFGHIJLMNOPQRSTUVWXYZ");

        public static PolybiusSquare CreateFromKeyword(string keyword, Alphabet alphabet)
        {
            return CreateFromString(
                keyword.Select(Char.ToUpperInvariant)
                .Concat(alphabet)
                .Distinct()
                .ToArray()
            );
        }

        public static PolybiusSquare CreateFromAlphabet(Alphabet alphabet) => CreateFromString(alphabet.ToString());

        public static PolybiusSquare CreateFromString(ReadOnlySpan<char> source)
        {
            int size = (int)Math.Sqrt(source.Length);

            char[,] result = new char[size, size];

            for (int i = 0; i < source.Length; i++)
                result[i % size, i / size] = source[i];

            return result;
        }

        public static (int x, int y) FindOffsets(char[,] polybiusSquare, char ch)
        {
            int width = polybiusSquare.GetLength(0), height = polybiusSquare.GetLength(1);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (polybiusSquare[x, y] == ch)
                        return (x, y);

            return (-1, -1);
        }

        public static bool TryFindOffsets(char[,] polybiusSquare, char ch, out (int x, int y) positions)
        {
            int width = polybiusSquare.GetLength(0), height = polybiusSquare.GetLength(1);

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    if (polybiusSquare[x, y] == ch)
                    {
                        positions = (x, y);
                        return true;
                    }

            positions = default;
            return false;
        }

        public char[,] ToCharArray() => (char[,])_data.Clone();
    }
}
