using System;
using System.Composition;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

using TVector = System.Runtime.Intrinsics.Vector128<short>;

namespace Science.Cryptography.Ciphers.Specialized;

/// <summary>
/// Represents the Atbash cipher.
/// </summary>
[Export("ASCII-Atbash", typeof(ICipher))]
public class AsciiAtbashCipher : ReciprocalCipher
{
    private const int APlusZ = (int)('A' + 'Z');
    private const int LowercaseAPlusZ = (int)('a' + 'z');

	private static readonly TVector VectorOfAPlusZ = Vector128.Create((short)('A' + 'Z'));
	private static readonly TVector VectorOfLowercaseAPlusZ = Vector128.Create((short)('a' + 'z'));
	private static readonly TVector VectorOfAMinus1 = Vector128.Create((short)('A' - 1));
	private static readonly TVector VectorOfZPlus1 = Vector128.Create((short)('Z' + 1));
	private static readonly TVector VectorOfLowercaseAMinus1 = Vector128.Create((short)('a' - 1));
	private static readonly TVector VectorOfLowercaseZPlus1 = Vector128.Create((short)('z' + 1));

	protected override void Crypt(ReadOnlySpan<char> text, Span<char> result, out int written)
	{
		if (result.Length < text.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(result));
		}

        if (Avx2.IsSupported)
        {
            // process the vectorized input
            var vectorCount = text.Length / TVector.Count;
            var totalVectorizedLength = vectorCount * TVector.Count;
            var vectorizedText = MemoryMarshal.Cast<char, short>(text);
            var vectorizedResult = MemoryMarshal.Cast<char, short>(result);
            for (int offset = 0; offset < totalVectorizedLength; offset += TVector.Count)
            {
                var input = Vector128.LoadUnsafe(ref MemoryMarshal.GetReference(vectorizedText[offset..]));
                var output = CryptBlockAvx2(input);
                output.StoreUnsafe(ref MemoryMarshal.GetReference(vectorizedResult[offset..]));
            }

            // process the remaining input
            if (totalVectorizedLength < text.Length)
            {
                var remainingInput = text[totalVectorizedLength..];
                var remainingOutput = result[totalVectorizedLength..];
                CryptSlow(remainingInput, remainingOutput);
            }
        }
        else
        {
            CryptSlow(text, result);
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
	public static TVector CryptBlockAvx2(TVector input)
	{
		// uppercase
		var isUpperMask = Avx2.And(
			Avx2.CompareGreaterThan(input, VectorOfAMinus1),
			Avx2.CompareLessThan(input, VectorOfZPlus1)
		);
		TVector transformedUppercase;
		if (isUpperMask == TVector.Zero)
		{
			transformedUppercase = input;
		}
		else
		{
			transformedUppercase = Avx2.Subtract(VectorOfAPlusZ, input);
			transformedUppercase = Avx2.BlendVariable(input, transformedUppercase, isUpperMask);
		}

		// lowercase
		var isLowerMask = Avx2.And(
			Avx2.CompareGreaterThan(input, VectorOfLowercaseAMinus1),
			Avx2.CompareLessThan(input, VectorOfLowercaseZPlus1)
		);
		TVector transformedLowercase;
		if (isLowerMask == TVector.Zero)
		{
			transformedLowercase = input;
		}
		else
		{
			transformedLowercase = Avx2.Subtract(VectorOfLowercaseAPlusZ, input);
			transformedLowercase = Avx2.BlendVariable(transformedLowercase, transformedLowercase, isLowerMask);
		}

		// merge
		var transformed = Avx2.BlendVariable(transformedUppercase, transformedLowercase, isLowerMask);

		return transformed;
	}
}