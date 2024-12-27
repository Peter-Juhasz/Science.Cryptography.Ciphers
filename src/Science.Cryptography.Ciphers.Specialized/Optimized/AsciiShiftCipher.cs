using System;
using System.Composition;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

using TVector = System.Runtime.Intrinsics.Vector256<short>;

namespace Science.Cryptography.Ciphers.Specialized;

/// <summary>
/// Represents the Shift cipher.
/// </summary>
[Export("ASCII-Shift", typeof(IKeyedCipher<>))]
public class AsciiShiftCipher : IKeyedCipher<int>
{
	private const short AlphabetLength = 'Z' - 'A' + 1;

	private static readonly TVector VectorOfA = Vector256.Create((short)'A');
	private static readonly TVector VectorOfAMinus1 = Vector256.Create((short)('A' - 1));
	private static readonly TVector VectorOfZ = Vector256.Create((short)'Z');
	private static readonly TVector VectorOfZPlus1 = Vector256.Create((short)('Z' + 1));
	private static readonly TVector VectorOfLowercaseA = Vector256.Create((short)'a');
	private static readonly TVector VectorOfLowercaseAMinus1 = Vector256.Create((short)('a' - 1));
	private static readonly TVector VectorOfLowercaseZ = Vector256.Create((short)'z');
	private static readonly TVector VectorOfLowercaseZPlus1 = Vector256.Create((short)('z' + 1));

	public void Encrypt(ReadOnlySpan<char> text, Span<char> result, int key, out int written)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(key, 0, paramName: nameof(key));

		if (result.Length < text.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(result));
		}

		if (key >= AlphabetLength)
		{
			key %= AlphabetLength;
		}

		if (key == 0)
		{
			text.CopyTo(result);
			written = text.Length;
			return;
		}

		var totalVectorizedLength = 0;

		// process the vectorized input
		if (Avx2.IsSupported && Vector256.IsHardwareAccelerated)
		{
			var vectorCount = text.Length / TVector.Count;
			totalVectorizedLength = vectorCount * TVector.Count;
			var inputAsShort = MemoryMarshal.Cast<char, short>(text);
			var outputAsShort = MemoryMarshal.Cast<char, short>(result);
			var keyVector = Vector256.Create((short)key);
			for (int offset = 0; offset < totalVectorizedLength; offset += TVector.Count)
			{
				var inputBlock = Unsafe.As<short, TVector>(ref MemoryMarshal.GetReference(inputAsShort[offset..]));
				var outputBlock = EncryptBlockAvx2(inputBlock, keyVector);
				outputBlock.StoreUnsafe(ref MemoryMarshal.GetReference(outputAsShort[offset..]));
			}
		}

		// process the remaining input
		if (totalVectorizedLength < text.Length)
		{
			var remainingInput = text[totalVectorizedLength..];
			var remainingOutput = result[totalVectorizedLength..];
			EncryptSlow(remainingInput, remainingOutput, key);
		}

		written = text.Length;
	}

	public void Decrypt(ReadOnlySpan<char> text, Span<char> result, int key, out int written)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(key, 0, paramName: nameof(key));

		if (result.Length < text.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(result));
		}

		if (key >= AlphabetLength)
		{
			key %= AlphabetLength;
		}

		if (key == 0)
		{
			text.CopyTo(result);
			written = text.Length;
			return;
		}

		var totalVectorizedLength = 0;

		// process the vectorized input
		if (Avx2.IsSupported && Vector256.IsHardwareAccelerated)
		{
			var vectorCount = text.Length / TVector.Count;
			totalVectorizedLength = vectorCount * TVector.Count;
			var inputAsShort = MemoryMarshal.Cast<char, short>(text);
			var outputAsShort = MemoryMarshal.Cast<char, short>(result);
			var keyVector = Vector256.Create((short)key);
			for (int offset = 0; offset < totalVectorizedLength; offset += TVector.Count)
			{
				var inputBlock = Unsafe.As<short, TVector>(ref MemoryMarshal.GetReference(inputAsShort[offset..]));
				var outputBlock = DecryptBlockAvx2(inputBlock, keyVector);
				outputBlock.StoreUnsafe(ref MemoryMarshal.GetReference(outputAsShort[offset..]));
			}
		}

		// process the remaining input
		if (totalVectorizedLength < text.Length)
		{
			var remainingInput = text[totalVectorizedLength..];
			var remainingOutput = result[totalVectorizedLength..];
			DecryptSlow(remainingInput, remainingOutput, key);
		}

		written = text.Length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static TVector EncryptBlockAvx2(TVector input, TVector keyVector)
	{
		// uppercase
		var isUpperMask = Avx2.AndNot(
			Avx2.CompareGreaterThan(input, VectorOfZ),
			Avx2.CompareGreaterThan(input, VectorOfAMinus1)
		);
		TVector transformedUppercase;
		if (isUpperMask == TVector.Zero)
		{
			transformedUppercase = input;
		}
		else
		{
			transformedUppercase = Avx2.Add(input, keyVector);

			// normalize overflow
			var uppercaseOverflowMask = Avx2.And(Avx2.CompareGreaterThan(transformedUppercase, VectorOfZ), isUpperMask);
			var uppercaseOverflow = Avx2.Subtract(transformedUppercase, VectorOfZPlus1);
			var uppercaseNormalized = Avx2.Add(VectorOfA, uppercaseOverflow);
			transformedUppercase = Avx2.BlendVariable(transformedUppercase, uppercaseNormalized, uppercaseOverflowMask);
		
			transformedUppercase = Avx2.BlendVariable(input, transformedUppercase, isUpperMask);
		}

		// lowercase
		var isLowerMask = Avx2.AndNot(
			Avx2.CompareGreaterThan(input, VectorOfLowercaseZ),
			Avx2.CompareGreaterThan(input, VectorOfLowercaseAMinus1)
		);
		TVector transformedLowercase;
		if (isLowerMask == TVector.Zero)
		{
			transformedLowercase = input;
		}
		else
		{
			transformedLowercase = Avx2.Add(input, keyVector);

			// normalize overflow
			var lowercaseOverflowMask = Avx2.And(Avx2.CompareGreaterThan(transformedLowercase, VectorOfLowercaseZ), isLowerMask);
			var lowercaseOverflow = Avx2.Subtract(transformedLowercase, VectorOfLowercaseZPlus1);
			var lowercaseNormalized = Avx2.Add(VectorOfLowercaseA, lowercaseOverflow);
			transformedLowercase = Avx2.BlendVariable(transformedLowercase, lowercaseNormalized, lowercaseOverflowMask);

			transformedLowercase = Avx2.BlendVariable(transformedLowercase, transformedLowercase, isLowerMask);
		}

		// merge
		var transformed = Avx2.BlendVariable(transformedUppercase, transformedLowercase, isLowerMask);

		return transformed;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static TVector DecryptBlockAvx2(TVector input, TVector keyVector)
	{
		// uppercase
		var isUpperMask = Avx2.AndNot(
			Avx2.CompareGreaterThan(input, VectorOfZ),
			Avx2.CompareGreaterThan(input, VectorOfAMinus1)
		);
		TVector transformedUppercase;
		if (isUpperMask == TVector.Zero)
		{
			transformedUppercase = input;
		}
		else
		{
			transformedUppercase = Avx2.Subtract(input, keyVector);

			// normalize underflow
			var uppercaseUnderflowMask = Avx2.AndNot(Avx2.CompareGreaterThan(transformedUppercase, VectorOfAMinus1), isUpperMask);
			var uppercaseUnderflow = Avx2.Subtract(VectorOfA, transformedUppercase);
			var uppercaseNormalized = Avx2.Subtract(VectorOfZPlus1, uppercaseUnderflow);
			transformedUppercase = Avx2.BlendVariable(transformedUppercase, uppercaseNormalized, uppercaseUnderflowMask);

			transformedUppercase = Avx2.BlendVariable(input, transformedUppercase, isUpperMask);
		}

		// lowercase
		var isLowerMask = Avx2.AndNot(
			Avx2.CompareGreaterThan(input, VectorOfLowercaseZ),
			Avx2.CompareGreaterThan(input, VectorOfLowercaseAMinus1)
		);
		TVector transformedLowercase;
		if (isLowerMask == TVector.Zero)
		{
			transformedLowercase = input;
		}
		else
		{
			transformedLowercase = Avx2.Subtract(input, keyVector);

			// normalize underflow
			var lowercaseUnderflowMask = Avx2.AndNot(Avx2.CompareGreaterThan(transformedLowercase, VectorOfLowercaseAMinus1), isLowerMask);
			var lowercaseUnderflow = Avx2.Subtract(VectorOfLowercaseA, transformedLowercase);
			var lowercaseNormalized = Avx2.Subtract(VectorOfLowercaseZPlus1, lowercaseUnderflow);
			transformedLowercase = Avx2.BlendVariable(transformedLowercase, lowercaseNormalized, lowercaseUnderflowMask);

			transformedLowercase = Avx2.BlendVariable(transformedLowercase, transformedLowercase, isLowerMask);
		}

		// merge
		var transformed = Avx2.BlendVariable(transformedUppercase, transformedLowercase, isLowerMask);

		return transformed;
	}

	private static void EncryptSlow(ReadOnlySpan<char> text, Span<char> result, int key)
	{
		for (int i = 0; i < text.Length; i++)
		{
			var ch = text[i];

			// lowercase
			if (ch >= 'a')
			{
				if (ch > 'z')
				{
					result[i] = ch;
					continue;
				}

				var value = ch + key;

				// normalize overflow
				if (value > 'z')
				{
					value -= AlphabetLength;
				}

				result[i] = (char)value;
			}

			// uppercase
			else if (ch >= 'A')
			{
				if (ch > 'Z')
				{
					result[i] = ch;
					continue;
				}

				var value = ch + key;

				// normalize overflow
				if (value > 'Z')
				{
					value -= AlphabetLength;
				}

				result[i] = (char)value;
			}

			// non-alphabet
			else
			{
				result[i] = ch;
			}
		}
	}

	private static void DecryptSlow(ReadOnlySpan<char> text, Span<char> result, int key)
	{
		for (int i = 0; i < text.Length; i++)
		{
			var ch = text[i];

			// lowercase
			if (ch <= 'Z')
			{
				if (ch < 'A')
				{
					result[i] = ch;
					continue;
				}

				var value = ch - key;

				// normalize underflow
				if (value < 'A')
				{
					value += AlphabetLength;
				}

				result[i] = (char)value;
			}

			// uppercase
			else if (ch <= 'z')
			{
				if (ch < 'a')
				{
					result[i] = ch;
					continue;
				}

				var value = ch - key;

				// normalize underflow
				if (value < 'a')
				{
					value += AlphabetLength;
				}

				result[i] = (char)value;
			}

			// non-alphabet
			else
			{
				result[i] = ch;
			}
		}
	}
}
