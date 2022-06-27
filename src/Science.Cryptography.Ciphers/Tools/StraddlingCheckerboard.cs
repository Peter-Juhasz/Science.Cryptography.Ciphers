using System;

namespace Science.Cryptography.Ciphers;

public struct StraddlingCheckerboard
{
	private StraddlingCheckerboard(char[,] buffer, int[] indexes)
	{
		_buffer = buffer;
		_indexes = indexes;
		_height = buffer.GetLength(0);
	}

	public const int Width = 10;

	public const char EmptyValue = default;
	public const char FullStop = '.';
	public const char NumericEscape = '/';

	public const int EmptyIndex = default;

	private readonly char[,] _buffer;
	private readonly int[] _indexes;
	private readonly int _height;

	public int Height => _height;

	private int TranslateRowIndexFromNamedToNumerical(int rowNamedIndex) => rowNamedIndex switch
	{
		EmptyIndex => 0,
		_ when _indexes.AsSpan().IndexOf(rowNamedIndex) is int translated and not -1 => 1 + translated,
		_ => throw new ArgumentOutOfRangeException(nameof(rowNamedIndex))
	};

	private int TranslateRowIndexFromNumericalToNamed(int rowNumericalIndex) => rowNumericalIndex switch
	{
		0 => EmptyIndex,
		_ => _indexes[rowNumericalIndex - 1]
	};

	/// <summary>
	/// Gets a <see cref="char"/> at a specified position.
	/// </summary>
	/// <param name="row">The custom index of the row of the checkerboard.</param>
	/// <param name="column">The column of the checkerboard.</param>
	/// <returns></returns>
	public char this[int namedRow, int column] => _buffer[TranslateRowIndexFromNamedToNumerical(namedRow), column];

	public static StraddlingCheckerboard CreateFromCharacters(ReadOnlySpan<char> source, params int[] namedRowIndexes)
	{
		var (quotient, remainder) = Math.DivRem(source.Length, Width);
		if (remainder != 0)
		{
			throw new ArgumentOutOfRangeException(nameof(source), $"Size of input characters must be a multiplier of {Width}.");
		}

		var buffer = new char[quotient, Width];
		ArrayHelper.FillFast(buffer, source, quotient, Width);
		return new(buffer, namedRowIndexes);
	}

	public static StraddlingCheckerboard Create(char[,] source, params int[] namedRowIndexes)
	{
		if (source.GetLength(1) != 10)
		{
			throw new ArgumentOutOfRangeException(nameof(source));
		}

		if (source.GetLength(0) != 1 + namedRowIndexes.Length)
		{
			throw new ArgumentOutOfRangeException(nameof(source));
		}

		foreach (var rowIndex in namedRowIndexes.AsSpan())
		{
			if (source[0, rowIndex] != EmptyValue)
			{
				throw new ArgumentException($"Top row of source at index {rowIndex} must be empty.", nameof(source));
			}
		}

		return new(source, namedRowIndexes);
	}

	public bool TryFindOffsets(char ch, out (int namedRow, int column) position)
	{
		if (ArrayHelper.TryFindOffsets(_buffer, ch, out position, _height, Width))
		{
			position.namedRow = TranslateRowIndexFromNumericalToNamed(position.namedRow);
			return true;
		}

		return false;
	}

	public ReadOnlySpan<char> GetRow(int namedRowIndex) =>
		_buffer.AsSpan(_height, Width).Slice(TranslateRowIndexFromNamedToNumerical(namedRowIndex) * Width, Width);

	public char[,] ToCharArray() => (char[,])_buffer.Clone();
}
