using System;
using System.Linq;
using System.Text;

namespace Science.Cryptography.Ciphers
{
    public class TapCode : ICipher
	{
        protected readonly char[,] CipherData = new char[,] {
			{ 'A', 'B', 'C', 'D', 'E' },
			{ 'F', 'G', 'H', 'I', 'J' },
			{ 'L', 'M', 'N', 'O', 'P' },
			{ 'Q', 'R', 'S', 'T', 'U' },
			{ 'V', 'W', 'X', 'Y', 'Z' }
		};


		public string Encrypt(string plaintext)
		{
			return String.Join(" ", plaintext.ToCharArray().Select(this.LetterToCode));
		}

		public string Decrypt(string ciphertext)
		{
			int row = 0, column = 0;
			int values = 0;

			StringBuilder result = new StringBuilder();

			foreach (char c in ciphertext)
			{
				// meta character
				if (c == '.')
				{
					// 
					if (values == 0)
						row++;
					else if (values == 1)
						column++;
				}

				// every other characters
				else if (c == ' ')
				{
					// step dimension
					values++;

					// if both dimensions are ready
					if (values == 2)
					{
						result.Append(CipherData[row - 1, column - 1]);
						
						row = column = 0;
						values = 0;
					}
				}
			}

			if (row != 0 && column != 0)
				result.Append(CipherData[row - 1, column - 1]);

			return result.ToString();
		}


		private string LetterToCode(char ch)
		{
			ch = Char.ToUpper(ch);

			for (int i = 1; i <= 5; i++)
			for (int j = 1; j <= 5; j++)
				if (CipherData[i - 1, j - 1] == ch)
					return String.Concat(new String('.', i), ' ', new String('.', j));

			return String.Empty;
		}
	}
}
