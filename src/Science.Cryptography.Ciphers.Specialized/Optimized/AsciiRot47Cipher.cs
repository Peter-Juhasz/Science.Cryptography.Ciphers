using System;
using System.Composition;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

using TVector = System.Runtime.Intrinsics.Vector256<short>;

namespace Science.Cryptography.Ciphers.Specialized;

/// <summary>
/// Represents the Atbash cipher.
/// </summary>
[Export("ASCII-ROT-47", typeof(ICipher))]
public class AsciiRot47Cipher : ReciprocalCipher
{
	private static readonly TVector VectorOfSpace = Vector256.Create((short)' ');
	private static readonly TVector VectorOf47 = Vector256.Create((short)47);
	private static readonly TVector VectorOf126 = Vector256.Create((short)126);
	private static readonly TVector VectorOf33 = Vector256.Create((short)33);
	private static readonly TVector VectorOf94 = Vector256.Create((short)94);

	protected override void Crypt(ReadOnlySpan<char> text, Span<char> result, out int written)
	{
		if (result.Length < text.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(result));
		}

		var totalVectorizedLength = 0;

		// process vectorized
		if (Avx2.IsSupported && Vector256.IsHardwareAccelerated)
        {
            var vectorCount = text.Length / TVector.Count;
            totalVectorizedLength = vectorCount * TVector.Count;
            var inputAsShort = MemoryMarshal.Cast<char, short>(text);
            var outputAsShort = MemoryMarshal.Cast<char, short>(result);
            for (int offset = 0; offset < totalVectorizedLength; offset += TVector.Count)
            {
                var inputBlock = Vector256.LoadUnsafe(ref MemoryMarshal.GetReference(inputAsShort[offset..]));
                var outputBlock = CryptBlockAvx2(inputBlock);
                outputBlock.StoreUnsafe(ref MemoryMarshal.GetReference(outputAsShort[offset..]));
            }
        }

		// process the remaining input
		if (totalVectorizedLength < text.Length)
		{
			var remainingInput = text[totalVectorizedLength..];
			var remainingOutput = result[totalVectorizedLength..];
			CryptSlow(remainingInput, remainingOutput);
		}

		written = text.Length;
	}

	internal static void CryptSlow(ReadOnlySpan<char> text, Span<char> result)
	{
		for (int i = 0; i < text.Length; i++)
		{
			var ch = text[i];
			if (ch == ' ')
			{
				result[i] = ' ';
				continue;
			}

			int value = ch + 47;

			if (value > 126)
			{
				value -= 94;
			}
			else if (value < 33)
			{
				value += 94;
			}

			result[i] = (char)value;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static TVector CryptBlockAvx2(TVector input)
	{
		// whitespace mask
		var spaceMask = Avx2.CompareEqual(VectorOfSpace, input);

		// add 47
		var transformed = Avx2.Add(input, VectorOf47);

		// subtract 94 if greater than 126
		var greaterThan126Mask = Avx2.CompareGreaterThan(transformed, VectorOf126);
		var subtracted = Avx2.Subtract(transformed, VectorOf94);
		transformed = Avx2.BlendVariable(transformed, subtracted, greaterThan126Mask);

		// add 94 if less than 33
		var lessThan33Mask = Avx2.CompareGreaterThan(VectorOf33, transformed);
		var added = Avx2.Add(transformed, VectorOf94);
		transformed = Avx2.BlendVariable(transformed, added, lessThan33Mask);

		// restore whitespace
		transformed = Avx2.BlendVariable(transformed, VectorOfSpace, spaceMask);

		return transformed;
	}
}