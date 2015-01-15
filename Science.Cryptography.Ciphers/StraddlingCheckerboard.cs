using System;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
	/// <summary>
	/// Contains methods for creating a manipulating straddling checkerboards.
	/// </summary>
	public static class StraddlingCheckerboard
	{
		private const int width = 10, height = 3;

		/// <summary>
		/// Creates a straddling checkerboard from a char array.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static char[,] CreateFromCharArray(char[] source)
		{
			char[,] result = new char[width, height];

			for (int i = 0; i < source.Length; i++)
				result[i % width, i / height] = source[i];

			return result;
		}

		/// <summary>
		/// Creates a straddling checkerboard from a string.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static char[,] CreateFromString(string source)
		{
			char[,] result = new char[width, height];

			for (int i = 0; i < source.Length; i++)
				result[i % width, i / height] = source[i];

			return result;
		}

		/// <summary>
		/// Creates a straddling checkerboard from a keyword, based on a custom alphabet.
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="alphabet"></param>
		/// <returns></returns>
		public static char[,] CreateFromKeyboard(string keyword, string alphabet)
		{
			return CreateFromCharArray(
				keyword.ToCharArray().Select(Char.ToUpper)
				    .Concat(alphabet.ToCharArray())
				    .Distinct()
				    .ToArray()
			);
		}

		/// <summary>
		/// Creates a straddling checkerboard from a keyword, based on the default alphabet.
		/// </summary>
		/// <param name="keyword"></param>
		/// <returns></returns>
		public static char[,] CreateFromKeyboard(string keyword)
		{
			throw new NotImplementedException();
		}

		public static int FindOffsets(char[,] straddlingCheckerboard, char ch)
		{
			int width = straddlingCheckerboard.GetLength(0),
                height = straddlingCheckerboard.GetLength(1);

			for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				if (straddlingCheckerboard[x, y] == ch)
					return y * 10 + x;

			return -1;
		}
	}
}
