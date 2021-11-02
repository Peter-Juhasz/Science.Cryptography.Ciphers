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

    public char this[int column, int row] => Alphabet.AtMod(column + row);
    public char this[char column, char row]
    {
        get
        {
            int i1 = this.Alphabet.IndexOfIgnoreCase(column),
                i2 = this.Alphabet.IndexOfIgnoreCase(row);

            return this[i1, i2];
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
            row = string.Concat(Alphabet[index..], Alphabet[..index]);
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
    public char FindColumnOrRowLabel(char columnOrRow, char @char) => Alphabet.AtMod(Alphabet.IndexOfIgnoreCase(@char) - Alphabet.IndexOfIgnoreCase(columnOrRow));
}
