using System;
using System.Composition;

namespace Science.Cryptography.Ciphers.Specialized;

[Export("Reverse", typeof(ICipher))]
public class ReverseCipher : ReciprocalCipher
{
	protected override void Crypt(ReadOnlySpan<char> input, Span<char> output, out int written)
	{
		input.CopyTo(output);
		output[..input.Length].Reverse();
		written = input.Length;
	}
}
