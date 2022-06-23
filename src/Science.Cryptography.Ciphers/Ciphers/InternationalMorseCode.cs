using System;
using System.Collections.Generic;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Morse Code cipher.
/// </summary>
[Export("Morse Code", typeof(ICipher))]
public class InternationalMorseCode : ICipher
{
	public InternationalMorseCode(InternationalMorseCodeOptions options)
	{
		Options = options;
		_map = GetSubstitutionMap(Options);
	}
	public InternationalMorseCode()
		: this(new(Dot: '.', Dash: '-', Delimiter: ' '))
	{ }

	public InternationalMorseCodeOptions Options { get; }
	private readonly CharacterToSegmentSubstitutionMap _map;

	public int MaxOutputCharactersPerInputCharacter => 6 + 1;

	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written)
	{
		var delimiter = Options.Delimiter;
		var writeDelimiter = false;

		var writer = new SpanWriter<char>(ciphertext);

		foreach (var ch in plaintext)
		{
			if (_map.TryLookup(ch, out var code))
			{
				if (writeDelimiter)
				{
					writer.Write(delimiter);
				}

				writer.Write(code);
			}
			else
			{
				writer.Write(ch);
			}

			writeDelimiter = true;
		}

		written = writer.Written;
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written)
	{
		var dot = Options.Dot;
		var dash = Options.Dash;

		int start = 0, end = 0;

		var writtenPosition = 0;

		for (int i = 0; i < ciphertext.Length; i++)
		{
			var ch = ciphertext[i];
			if (ch == dot || ch == dash)
			{
				if (start == end)
				{
					start = i;
				}
				end = i + 1;
			}
			else
			{
				if (end == i && start < end && _map.TryReverseLookup(ciphertext[start..end], out var tr))
				{
					plaintext[writtenPosition++] = tr;
				}
				else
				{
					plaintext[writtenPosition++] = ch;
				}
				start = end = 0;
			}
		}

		if (end == ciphertext.Length && start < end && _map.TryReverseLookup(ciphertext[start..end], out var tr2))
		{
			plaintext[writtenPosition++] = tr2;
		}

		written = writtenPosition;
	}


	// https://www.itu.int/dms_pubrec/itu-r/rec/m/R-REC-M.1677-1-200910-I!!PDF-E.pdf
	private static CharacterToSegmentSubstitutionMap GetSubstitutionMap(InternationalMorseCodeOptions options) => options switch
	{
		(char Dot, char Dash, _) => new(new Dictionary<char, string>(IgnoreCaseCharComparer.Instance)
		{
			{ 'A', $"{Dot}{Dash}" },
			{ 'B', $"{Dash}{Dot}{Dot}{Dot}" },
			{ 'C', $"{Dash}{Dot}{Dash}{Dot}" },
			{ 'D', $"{Dash}{Dot}{Dot}" },
			{ 'E', $"{Dot}" },
			{ 'F', $"{Dot}{Dot}{Dash}{Dot}" },
			{ 'G', $"{Dash}{Dash}{Dot}" },
			{ 'H', $"{Dot}{Dot}{Dot}{Dot}" },
			{ 'I', $"{Dot}{Dot}" },
			{ 'J', $"{Dot}{Dash}{Dash}{Dash}" },
			{ 'K', $"{Dash}{Dot}{Dash}" },
			{ 'L', $"{Dot}{Dash}{Dot}{Dot}" },
			{ 'M', $"{Dash}{Dash}" },
			{ 'N', $"{Dash}{Dot}" },
			{ 'O', $"{Dash}{Dash}{Dash}" },
			{ 'P', $"{Dot}{Dash}{Dash}{Dot}" },
			{ 'Q', $"{Dash}{Dash}{Dot}{Dash}" },
			{ 'R', $"{Dot}{Dash}{Dot}" },
			{ 'S', $"{Dot}{Dot}{Dot}" },
			{ 'T', $"{Dash}" },
			{ 'U', $"{Dot}{Dot}{Dash}" },
			{ 'V', $"{Dot}{Dot}{Dot}{Dash}" },
			{ 'W', $"{Dot}{Dash}{Dash}" },
			{ 'X', $"{Dash}{Dot}{Dot}{Dash}" },
			{ 'Y', $"{Dash}{Dot}{Dash}{Dash}" },
			{ 'Z', $"{Dash}{Dash}{Dot}{Dot}" },

			{ '1', $"{Dot}{Dash}{Dash}{Dash}{Dash}" },
			{ '2', $"{Dot}{Dot}{Dash}{Dash}{Dash}" },
			{ '3', $"{Dot}{Dot}{Dot}{Dash}{Dash}" },
			{ '4', $"{Dot}{Dot}{Dot}{Dot}{Dash}" },
			{ '5', $"{Dot}{Dot}{Dot}{Dot}{Dot}" },
			{ '6', $"{Dash}{Dot}{Dot}{Dot}{Dot}" },
			{ '7', $"{Dash}{Dash}{Dot}{Dot}{Dot}" },
			{ '8', $"{Dash}{Dash}{Dash}{Dot}{Dot}" },
			{ '9', $"{Dash}{Dash}{Dash}{Dash}{Dot}" },
			{ '0', $"{Dash}{Dash}{Dash}{Dash}{Dash}" },

			{ '.', $"{Dot}{Dash}{Dot}{Dash}{Dot}{Dash}" },
			{ ',', $"{Dash}{Dash}{Dot}{Dot}{Dash}{Dash}" },
			{ ':', $"{Dash}{Dash}{Dash}{Dot}{Dot}{Dot}" },
			{ '?', $"{Dot}{Dot}{Dash}{Dash}{Dot}{Dot}" },
			{ '\'', $"{Dot}{Dash}{Dash}{Dash}{Dash}{Dot}" },
			{ '-', $"{Dash}{Dot}{Dot}{Dot}{Dot}{Dash}" },
			{ '/', $"{Dash}{Dot}{Dot}{Dash}{Dot}" },
			{ '(', $"{Dash}{Dot}{Dash}{Dash}{Dot}" },
			{ ')', $"{Dash}{Dot}{Dash}{Dash}{Dot}{Dash}" },
			{ '"', $"{Dot}{Dash}{Dot}{Dot}{Dash}{Dot}" },
			{ '=', $"{Dash}{Dot}{Dot}{Dot}{Dash}" },
			{ '+', $"{Dot}{Dash}{Dot}{Dash}{Dot}" },
			{ '×', $"{Dash}{Dot}{Dot}{Dash}" },
			{ '@', $"{Dot}{Dash}{Dash}{Dot}{Dash}{Dot}" },
		})
	};
}
