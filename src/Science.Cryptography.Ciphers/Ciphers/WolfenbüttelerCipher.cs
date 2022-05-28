using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

[Export("Wolfenbütteler", typeof(ICipher))]
[Export("Wolfenbuetteler", typeof(ICipher))]
public class WolfenbüttelerCipher : ReciprocalCipher
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

	private static char Translate(char ch) => (ch.ToUpper() switch
	{
		'A' => 'M',
		'M' => 'A',

		'E' => 'K',
		'K' => 'E',

		'I' => 'D',
		'D' => 'I',

		'O' => 'T',
		'T' => 'O',

		'U' => 'H',
		'H' => 'U',

		_ => ch
	}).ToSameCaseAs(ch);
}
