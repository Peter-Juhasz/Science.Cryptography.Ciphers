using System;
using System.Linq;

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

	public char this[ValueTuple<int, int> position] => _data[position.Item1, position.Item2];

	public bool TryFindOffsets(char ch, out ValueTuple<int, int> position) => ArrayHelper.TryFindOffsets(_data, ch, out position, _size);

	public bool Contains(char ch) => TryFindOffsets(ch, out _);


	public static implicit operator PolybiusSquare(char[,] s) => new(s);


	public static readonly PolybiusSquare RegularWithoutI = CreateFromAlphabet(WellKnownAlphabets.EnglishWithoutI);

	public static readonly PolybiusSquare RegularWithoutK = CreateFromAlphabet(WellKnownAlphabets.EnglishWithoutK);

	public static PolybiusSquare CreateFromKeyword(string keyword, Alphabet alphabet)
	{
		// TODO: optimize allocations
		return CreateFromString(
			keyword.Select(Char.ToUpperInvariant)
			.Concat(alphabet)
			.Distinct()
			.ToArray()
		);
	}

	public static PolybiusSquare CreateFromAlphabet(Alphabet alphabet) => CreateFromString(alphabet.AsSpan());

	public static PolybiusSquare CreateFromString(ReadOnlySpan<char> source)
	{
		int size = (int)Math.Sqrt(source.Length);

		char[,] result = new char[size, size];
		ArrayHelper.FillSlow(result, source, size);
		return result;
	}

	public static bool TryFindOffsets(char[,] polybiusSquare, char ch, out ValueTuple<int, int> positions) =>
		ArrayHelper.TryFindOffsets(polybiusSquare, ch, out positions, polybiusSquare.GetLength(0), polybiusSquare.GetLength(1));

	public char[,] ToCharArray() => (char[,])_data.Clone();
}
