using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Morse Code cipher.
/// </summary>
[Export("Morse Code", typeof(ICipher))]
public class MorseCode : ICipher
{
	public MorseCode(MorseCodeOptions options)
	{
		Options = options;
		_map = options.GetSubstitutionMap();
	}
	public MorseCode()
		: this(MorseCodeOptions.Default)
	{ }

	public MorseCodeOptions Options { get; }
	private readonly CharacterToSegmentSubstitutionMap _map;

	public int MaxOutputCharactersPerInputCharacter => 4 + 1;

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
}