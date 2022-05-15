using System;
using System.Composition;
using System.Text;

namespace Science.Cryptography.Ciphers.Specialized;

/// <summary>
/// Represents the XOR cipher.
/// </summary>
[Export("ASCII-XOR", typeof(IKeyedCipher<>))]
public class AsciiXorCipher : ReciprocalKeyedCipher<byte[]>
{
	private static Encoding Encoding = Encoding.ASCII;

	protected override void Crypt(ReadOnlySpan<char> input, Span<char> output, byte[] key, out int written)
	{
		if (key.Length == 0)
		{
			throw new ArgumentException("Key can't be zero-length.", nameof(key));
		}

		if (output.Length < input.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(output));
		}

		// prepare buffer
		var length = input.Length;
		Span<byte> inputBytes = stackalloc byte[length];
		Encoding.GetBytes(input, inputBytes);

		Span<byte> outputBytes = stackalloc byte[length];

		// crypt
		BinaryXor.Xor(inputBytes, outputBytes, key);

		// decode
		written = Encoding.GetChars(outputBytes, output);
	}
}
