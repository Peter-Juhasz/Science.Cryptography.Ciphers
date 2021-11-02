using System;
using System.Linq;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Contains methods for creating a manipulating straddling checkerboards.
/// </summary>
public class StraddlingCheckerboard
{
    private StraddlingCheckerboard(char[,] buffer)
    {
        _buffer = buffer;
    }

    public const int Width = 10, Height = 3;

    private readonly char[,] _buffer;

    /// <summary>
    /// Gets a <see cref="char"/> at a specified position.
    /// </summary>
    /// <param name="row">The row of the checkerboard.</param>
    /// <param name="column">The column of the checkerboard.</param>
    /// <returns></returns>
    public char this[int column, int row] => _buffer[column, row];

    /// <summary>
    /// Creates a straddling checkerboard from a <see cref="char[]"/>.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static StraddlingCheckerboard CreateFromCharArray(char[] source) => new(CreateBufferFromString(source));

    /// <summary>
    /// Creates a straddling checkerboard from a <see cref="string"/>.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static StraddlingCheckerboard CreateFromString(string source) => new(CreateBufferFromString(source));

    public static StraddlingCheckerboard CreateFromAlphabet(Alphabet alphabet) => new(CreateBufferFromString(alphabet.ToString()));

    /// <summary>
    /// Creates a straddling checkerboard from a keyword, based on a custom charset.
    /// </summary>
    /// <param name="keyword"></param>
    /// <param name="charset"></param>
    /// <returns></returns>
    public static StraddlingCheckerboard CreateFromKeyword(string keyword, Alphabet alphabet) => new(CreateBufferFromKeyword(keyword, alphabet));


    internal static char[,] CreateBufferFromString(ReadOnlySpan<char> source)
    {
        char[,] result = new char[Width, Height];

        for (int i = 0; i < source.Length; i++)
            result[i % Width, i / Height] = source[i];

        return result;
    }

    internal static char[,] CreateBufferFromKeyword(string keyword, Alphabet charset)
    {
        return CreateBufferFromString(
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
    public char[,] ToCharArray() => (char[,])_buffer.Clone();
}
