using System;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Contains methods for creating and manipulating Polybius Squares.
/// </summary>
public struct PolybiusSquare
{
	internal PolybiusSquare(char[,] data)
	{
		_data = data;
		_size = data.GetLength(0);
	}

	private readonly char[,] _data;
	private readonly int _size;

	public int Size => _size;

	public char this[int column, int row] => _data[column, row];

	public char this[(int row, int column) position] => _data[position.row, position.column];

	public bool TryFindOffsets(char ch, out (int row, int column) position) => ArrayHelper.TryFindOffsets(_data, ch, out position, _size);

	public bool Contains(char ch) => TryFindOffsets(ch, out _);


	public static implicit operator PolybiusSquare(char[,] s) => new(s);


	public static readonly PolybiusSquare RegularWithoutI = CreateFromAlphabet(WellKnownAlphabets.EnglishWithoutI);

	public static readonly PolybiusSquare RegularWithoutK = CreateFromAlphabet(WellKnownAlphabets.EnglishWithoutK);

	public static readonly PolybiusSquare Greek = CreateFromCharacters("ΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩ\0");

	public static PolybiusSquare CreateFromKeyword(string keyword, Alphabet alphabet)
	{
		if (!TrySqrt(alphabet.Length, out var size))
		{
			throw new ArgumentOutOfRangeException(nameof(alphabet));
		}

		char[,] result = new char[size, size];
		ArrayHelper.FillWithKeywordAndAlphabet(result.AsSpan(size), keyword, alphabet.AsSpan());
		return result;
	}

	public static PolybiusSquare CreateFromAlphabet(Alphabet alphabet) => CreateFromCharacters(alphabet.AsSpan());

	public static PolybiusSquare CreateFromCharacters(ReadOnlySpan<char> source)
	{
		if (!TrySqrt(source.Length, out var size))
		{
			throw new ArgumentOutOfRangeException(nameof(source));
		}

		char[,] result = new char[size, size];
		ArrayHelper.FillFast(result, source, size);
		return result;
	}

	public static PolybiusSquare Create(char[,] source)
	{
		if (source.GetLength(0) != source.GetLength(1))
		{
			throw new ArgumentOutOfRangeException(nameof(source));
		}

		return source;
	}

	public static bool TryFindOffsets(char[,] polybiusSquare, char ch, out (int row, int column) positions) =>
		ArrayHelper.TryFindOffsets(polybiusSquare, ch, out positions, polybiusSquare.GetLength(0), polybiusSquare.GetLength(1));

	public char[,] ToCharArray() => (char[,])_data.Clone();


	private static bool TrySqrt(int n, out int root)
	{
		root = n switch
		{
			25 => 5,
			36 => 6,
			16 => 4,
			49 => 7,
			9 => 3,
			4 => 2,
			1 => 1,
			_ => -1
		};
		if (root != -1)
		{
			return true;
		}

		root = (int)Math.Sqrt(n);
		if (root * root == n)
		{
			return true;
		}

		return false;
	}
}
