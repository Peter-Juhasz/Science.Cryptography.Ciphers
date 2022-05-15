using System;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents a Tabula Recta.
/// </summary>
public class TabulaRecta
{
	public TabulaRecta(Alphabet alphabet)
	{
		this.Alphabet = alphabet;
		_rowColumnCache = new string[alphabet.Length];
	}


	public static readonly TabulaRecta Regular = new(WellKnownAlphabets.English);


	public string this[int index] => GetRowOrColumn(index);
	public string this[char @char] => GetRowOrColumn(Alphabet.IndexOfIgnoreCase(@char));

	public char this[int row, int column] => GetAt(Alphabet, row, column);
	public char this[char row, char column]
	{
		get
		{
			int i1 = this.Alphabet.IndexOfIgnoreCase(column),
				i2 = this.Alphabet.IndexOfIgnoreCase(row);

			return this[i2, i1];
		}
	}


	public Alphabet Alphabet { get; }

	private readonly string?[] _rowColumnCache;


	/// <summary>
	/// Gets a row or a column of the tabula recta.
	/// </summary>
	/// <param name="index">The index of the row or the column.</param>
	/// <returns></returns>
	public string GetRowOrColumn(int index)
	{
		if (index < 0 || index >= Alphabet.Length)
			throw new ArgumentOutOfRangeException(nameof(index));

		if (_rowColumnCache[index] is not string row)
		{
			row = String.Create<object>(Alphabet.Length, null, (buffer, state) => GetRowOrColumn(Alphabet, index, buffer));
			_rowColumnCache[index] = row;
		}

		return row;
	}

	public string GetRowOrColumn(char @char) => GetRowOrColumn(Alphabet.IndexOfIgnoreCase(@char));

	/// <summary>
	/// Finds the corresponding column or row label for a character in a given column or row.
	/// </summary>
	/// <param name="columnOrRow"></param>
	/// <param name="char"></param>
	/// <returns></returns>
	public char FindColumnOrRowLabel(char columnOrRow, char @char) => FindColumnOrRowLabel(Alphabet, columnOrRow, @char);


	public static char GetAt(Alphabet alphabet, int row, int column) => alphabet.AtMod(row + column);

	public static char FindColumnOrRowLabel(Alphabet alphabet, char columnOrRow, char @char) => alphabet.AtMod(alphabet.IndexOfIgnoreCase(@char) - alphabet.IndexOfIgnoreCase(columnOrRow));

	public static void GetRowOrColumn(Alphabet alphabet, int index, Span<char> target)
	{
		alphabet.AsSpan()[index..].CopyTo(target);
		alphabet.AsSpan()[..index].CopyTo(target[index..]);
	}
}
