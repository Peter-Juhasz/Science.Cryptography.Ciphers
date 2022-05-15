using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents Sir Francis Beaufort's cipher.
/// </summary>
[Export("Beaufort", typeof(IKeyedCipher<>))]
public class BeaufortCipher : ReciprocalKeyedCipher<string>
{
	public BeaufortCipher(Alphabet alphabet)
	{
		Alphabet = alphabet;
		_tabulaRecta = new TabulaRecta(alphabet);
	}
	public BeaufortCipher()
		: this(WellKnownAlphabets.English)
	{ }

	private readonly TabulaRecta _tabulaRecta;

	public Alphabet Alphabet { get; }

	protected override void Crypt(ReadOnlySpan<char> input, Span<char> output, string key, out int written)
	{
		if (output.Length < input.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(output));
		}

		int charCounter = 0;
		for (int i = 0; i < input.Length; i++)
		{
			var ch = input[i];
			output[i] = Alphabet.IndexOfIgnoreCase(ch) switch
			{
				-1 => ch,
				_ => _tabulaRecta.FindColumnOrRowLabel(ch, key[charCounter++ % key.Length]).ToSameCaseAs(ch)
			};
		}

		written = input.Length;
	}
}
