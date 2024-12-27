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
[Export("ASCII-Atbash", typeof(ICipher))]
public class AsciiAtbashCipher : ReciprocalCipher
{
    private const int APlusZ = (int)('A' + 'Z');
    private const int LowercaseAPlusZ = (int)('a' + 'z');

	private static readonly TVector VectorOfAPlusZ = Vector256.Create((short)('A' + 'Z'));
	private static readonly TVector VectorOfLowercaseAPlusZ = Vector256.Create((short)('a' + 'z'));
	private static readonly TVector VectorOfAMinus1 = Vector256.Create((short)('A' - 1));
	private static readonly TVector VectorOfZ = Vector256.Create((short)'Z');
	private static readonly TVector VectorOfLowercaseAMinus1 = Vector256.Create((short)('a' - 1));
	private static readonly TVector VectorOfLowercaseZ = Vector256.Create((short)'z');

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
			var idx = (int)ch;
			result[i] = ch switch
			{
				>= 'A' and <= 'Z' => (char)(APlusZ - idx),
				>= 'a' and <= 'z' => (char)(LowercaseAPlusZ - idx),
				_ => ch,
			};
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static TVector CryptBlockAvx2(TVector input)
	{
		// uppercase
		var isUpperMask = Avx2.AndNot(
			Avx2.CompareGreaterThan(input, VectorOfZ),
			Avx2.CompareGreaterThan(input, VectorOfAMinus1)
		);
		var transformedUppercase = Avx2.Subtract(VectorOfAPlusZ, input);
		transformedUppercase = Avx2.BlendVariable(input, transformedUppercase, isUpperMask);

		// lowercase
		var isLowerMask = Avx2.AndNot(
			Avx2.CompareGreaterThan(input, VectorOfLowercaseZ),
			Avx2.CompareGreaterThan(input, VectorOfLowercaseAMinus1)
		);
		var transformedLowercase = Avx2.Subtract(VectorOfLowercaseAPlusZ, input);
		transformedLowercase = Avx2.BlendVariable(transformedLowercase, transformedLowercase, isLowerMask);

		// merge
		var transformed = Avx2.BlendVariable(transformedUppercase, transformedLowercase, isLowerMask);

		return transformed;
	}
}