using System;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents a Tabula Recta.
    /// </summary>
    public class TabulaRecta
    {
        public TabulaRecta(string charset = Charsets.English)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            this.Charset = charset;
        }


        public static readonly TabulaRecta Regular = new TabulaRecta();


        public string this[int index]
        {
            get { return this.GetRowOrColumn(index); }
        }
        public string this[char @char]
        {
            get { return this.GetRowOrColumn(this.Charset.IndexOfIgnoreCase(@char)); }
        }

        public char this[int column, int row]
        {
            get { return this.GetRowOrColumn(column)[row]; }
        }
        public char this[char column, char row]
        {
            get
            {
                int i1 = this.Charset.IndexOfIgnoreCase(column),
                    i2 = this.Charset.IndexOfIgnoreCase(row);

                return this.GetRowOrColumn(i1)[i2];
            }
        }


        public string Charset { get; private set; }


        /// <summary>
        /// Gets a row or a column of the tabula recta.
        /// </summary>
        /// <param name="index">The index of the row or the column.</param>
        /// <returns></returns>
        public string GetRowOrColumn(int index)
        {
            if (index >= this.Charset.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            return this.Charset.Substring(index) + this.Charset.Substring(0, index);
        }
        public string GetRowOrColumn(char @char)
        {
            return this.GetRowOrColumn(this.Charset.IndexOfIgnoreCase(@char));
        }
    }
}
