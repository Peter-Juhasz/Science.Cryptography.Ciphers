using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the XOR cipher.
/// </summary>
[Export("XOR", typeof(IKeyedCipher<>))]
public class XorCipher : ReciprocalKeyedCipher<int[]>
{
	protected override void Crypt(ReadOnlySpan<char> input, Span<char> output, int[] key, out int written)
	{
		if (key is [])
		{
			throw new ArgumentException("Key can't be zero-length.", nameof(key));
		}

		if (output.Length < input.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(output));
		}

		for (int i = 0; i < input.Length; i++)
		{
			output[i] = (char)(input[i] ^ key[i % key.Length]);
		}

		written = input.Length;
	}
}
