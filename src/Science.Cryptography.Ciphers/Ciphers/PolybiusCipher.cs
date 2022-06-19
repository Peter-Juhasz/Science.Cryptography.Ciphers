using System;
using System.Composition;
using System.Text;

namespace Science.Cryptography.Ciphers;

[Export("Polybius", typeof(IKeyedCipher<PolybiusSquare>))]
public class PolybiusCipher : IKeyedCipher<PolybiusSquare>
{
	private static readonly Encoding ASCII = Encoding.ASCII;

	public int GetMaxOutputCharactersPerInputCharacter(PolybiusSquare key) => 2;

	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, PolybiusSquare key, out int written)
	{
		if (ciphertext.Length < plaintext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(ciphertext));
		}

		var writer = new SpanWriter<char>(ciphertext);
		int first = -1, second = -1;

		for (int i = 0; i < plaintext.Length; i++)
		{
			var ch = plaintext[i];
			if (key.Contains(ch))
			{
				if (first == -1)
				{
					first = i;
					continue;
				}
				else if (second == -1)
				{
					second = i;
					if (TryResolveEncrypt(key, (plaintext[first], ch), out (int f1, int f2, int s1, int s2) result))
					{
						result.f1.TryFormat(writer.GetSpan(1), out _);
						result.f2.TryFormat(writer.GetSpan(1), out _);
						if ((second - first) is int difference and > 1)
						{
							var target = writer.GetSpan(difference - 1);
							plaintext[(first + 1)..second].CopyTo(target);
						}
						result.s1.TryFormat(writer.GetSpan(1), out _);
						result.s2.TryFormat(writer.GetSpan(1), out _);
					}
					else
					{
						var target = writer.GetSpan(second - first + 1);
						plaintext[first..(second + 1)].CopyTo(target);
					}

					first = second = -1;
					continue;
				}
			}
			else if (first == -1)
			{
				writer.Write(ch);
			}
		}

		written = writer.Written;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, PolybiusSquare key, out int written)
	{
		if (plaintext.Length < ciphertext.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(ciphertext));
		}

		var writer = new SpanWriter<char>(plaintext);
		int first = -1, second = -1;

		for (int i = 0; i < ciphertext.Length; i++)
		{
			var ch = ciphertext[i];
			if (key.Contains(ch))
			{
				if (first == -1)
				{
					first = i;
					continue;
				}
				else if (second == -1)
				{
					second = i;
					if (TryResolveDecrypt(key, (ciphertext[first], ch), out (char c1, char c2) result))
					{
						writer.Write(result.c1.ToSameCaseAs(ciphertext[first]));
						if ((second - first) is int difference and > 1)
						{
							var target = writer.GetSpan(difference - 1);
							ciphertext[(first + 1)..second].CopyTo(target);
						}
						writer.Write(result.c2.ToSameCaseAs(ch));
					}
					else
					{
						var target = writer.GetSpan(second - first + 1);
						ciphertext[first..(second + 1)].CopyTo(target);
					}

					first = second = -1;
					continue;
				}
			}
			else if (first == -1)
			{
				writer.Write(ch);
			}
		}

		written = writer.Written;
		throw new NotImplementedException();
	}


	public static bool TryResolveEncrypt(
		PolybiusSquare square,
		(char first, char second) pair,
		out (int firstLetterRow, int firstLetterColumn, int secondLetterRow, int secondLetterColumn) result
	)
	{
		if (square.TryFindOffsets(pair.first, out (int row, int column) position1) &&
			square.TryFindOffsets(pair.second, out (int row, int column) position2))
		{
			if (position1.row != position2.row && position1.column != position2.column)
			{
				result = (position1.row + 1, position2.column + 1, position2.row + 1, position1.column + 1);
			}
			else if (position1.row == position2.row)
			{
				result = (position1.row + 1, position1.column < square.Size - 1 ? position1.column + 2 : 1, position2.row + 1, position2.column < square.Size - 1 ? position2.column + 2 : 1);
			}
			else if (position1.column == position2.column)
			{
				result = (position1.row < square.Size - 1 ? position1.row + 2 : 1, position1.column + 1, position2.row < square.Size - 1 ? position2.row + 2 : 1, position2.column + 1);
			}
			else
			{
				result = default;
				return false;
			}
			return true;
		}

		result = default;
		return false;
	}

	public static bool TryResolveDecrypt(
		PolybiusSquare square, 
		(char first, char second) pair, 
		out (char first, char second) result
	)
	{
		if (square.TryFindOffsets(pair.first, out (int row, int column) position1) &&
			square.TryFindOffsets(pair.second, out (int row, int column) position2))
		{
			if (position1.row != position2.row && position1.column != position2.column)
			{
				result = (square[position1.row, position2.column], square[position2.row, position1.column]);
			}
			else if (position1.row == position2.row)
			{
				result = (square[position1.row, position1.column > 0 ? position1.column - 1 : square.Size - 1], square[position2.row, position2.column > 0 ? position2.column - 1 : square.Size - 1]);
			}
			else if (position1.column == position2.column)
			{
				result = (square[position1.row > 0 ? position1.row - 1 : square.Size - 1, position1.column], square[position2.row > 0 ? position2.row - 1 : square.Size - 1, position2.column]);
			}
			else
			{
				result = default;
				return false;
			}
			return true;
		}

		result = default;
		return false;
	}
}
