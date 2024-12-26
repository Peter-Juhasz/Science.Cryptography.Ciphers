using System;
using System.Composition;

namespace Science.Cryptography.Ciphers.Specialized;

[Export("Malespin", typeof(ICipher))]
public class MalespinSlang : ReciprocalCipher
{
	protected override void Crypt(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		if (output.Length < input.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(output));
		}

		for (int i = 0; i < input.Length; i++)
		{
			var ch = input[i];
			output[i] = Translate(ch);
		}

		written = input.Length;
	}

	private static char Translate(char ch) => (ch.ToUpperInvariant() switch
	{
		'A' => 'E',
		'E' => 'A',

		'B' => 'T',
		'T' => 'B',

		'F' => 'G',
		'G' => 'F',

		'I' => 'O',
		'O' => 'I',

		'M' => 'P',
		'P' => 'M',

		_ => ch
	}).ToSameCaseAs(ch);
}
