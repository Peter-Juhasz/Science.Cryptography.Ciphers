using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

[Export("DoubleColumnarTransposition", typeof(IKeyedCipher<>))]
public class DoubleColumnarTranspositionCipher : IKeyedCipher<(int[], int[])>
{
	private readonly ColumnarTranspositionCipher _inner = new();

	public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, (int[], int[]) key, out int written)
	{
		Span<char> intermediate = stackalloc char[plaintext.Length];
		_inner.Encrypt(plaintext, intermediate, key.Item1, out written);
		_inner.Encrypt(intermediate[..written], ciphertext, key.Item2, out written);
	}

	public void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, (int[], int[]) key, out int written)
	{
		Span<char> intermediate = stackalloc char[plaintext.Length];
		_inner.Decrypt(ciphertext, intermediate, key.Item2, out written);
		_inner.Decrypt(intermediate[..written], plaintext, key.Item1, out written);
	}
}
