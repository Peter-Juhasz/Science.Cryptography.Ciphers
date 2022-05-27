using Science.Cryptography.Ciphers;
using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

[Export("TwoSquare", typeof(IKeyedCipher<(PolybiusSquare, PolybiusSquare)>))]
public class TwoSquareCipher : IKeyedCipher<(PolybiusSquare upperLeft, PolybiusSquare lowerRight)>
{
	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, (PolybiusSquare upperLeft, PolybiusSquare lowerRight) key, out int written)
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
			if (key.Item1.Contains(ch))
			{
				if (first == -1)
				{
					first = i;
					continue;
				}
				else if (second == -1)
				{
					second = i;
					if (TryResolve(key, (plaintext[first], ch), out (char c1, char c2) result))
					{
						writer.Write(result.c1.ToSameCaseAs(plaintext[first]));
						if ((second - first) is int difference and > 1)
						{
							var target = writer.GetSpan(difference - 1);
							plaintext[(first + 1)..second].CopyTo(target);
						}
						writer.Write(result.c2.ToSameCaseAs(ch));
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

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, (PolybiusSquare upperLeft, PolybiusSquare lowerRight) key, out int written)
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
			if (key.Item1.Contains(ch))
			{
				if (first == -1)
				{
					first = i;
					continue;
				}
				else if (second == -1)
				{
					second = i;
					if (TryResolve(key, (ciphertext[first], ch), out (char c1, char c2) result))
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
	}


	public static bool TryResolve((PolybiusSquare upperLeft, PolybiusSquare lowerRight) square, (char first, char second) pair, out (char first, char second) result)
	{
		if (square.Item1.TryFindOffsets(pair.first, out (int row, int column) position1) &&
			square.Item2.TryFindOffsets(pair.second, out (int row, int column) position2))
		{
			if (position1.column == position2.column)
			{
				result = pair;
			}
			else
			{
				result = (square.Item1[position1.row, position2.column], square.Item2[position2.row, position1.column]);
			}
			return true;
		}

		result = default;
		return false;
	}
}
