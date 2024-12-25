using System;
using System.Composition;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace Science.Cryptography.Ciphers.Specialized;

/// <summary>
/// Represents the Shift cipher.
/// </summary>
[Export("ASCII-Shift", typeof(IKeyedCipher<>))]
public class AsciiShiftCipher : IKeyedCipher<int>
{
	private const short AlphabetLength = 'Z' - 'A' + 1;

	private static readonly Vector128<short> VectorOfA = Vector128.Create((short)'A');
	private static readonly Vector128<short> VectorOfAMinus1 = Vector128.Create((short)('A' - 1));
	private static readonly Vector128<short> VectorOfZ = Vector128.Create((short)'Z');
	private static readonly Vector128<short> VectorOfZPlus1 = Vector128.Create((short)('Z' + 1));
	private static readonly Vector128<short> VectorOfLowercaseA = Vector128.Create((short)'a');
	private static readonly Vector128<short> VectorOfLowercaseAMinus1 = Vector128.Create((short)('a' - 1));
	private static readonly Vector128<short> VectorOfLowercaseZ = Vector128.Create((short)'z');
	private static readonly Vector128<short> VectorOfLowercaseZPlus1 = Vector128.Create((short)('z' + 1));

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

		if (Avx2.IsSupported)
		{
			// process the vectorized input
			var vectorCount = text.Length / Vector128<short>.Count;
			var totalVectorizedLength = vectorCount * Vector128<short>.Count;
			var vectorizedText = MemoryMarshal.Cast<char, short>(text);
			var vectorizedResult = MemoryMarshal.Cast<char, short>(result);
			var keyVector = Vector128.Create((short)key);
			for (int offset = 0; offset < totalVectorizedLength; offset += Vector128<short>.Count)
			{
				var input = Unsafe.As<short, Vector128<short>>(ref MemoryMarshal.GetReference(vectorizedText[offset..]));
				var output = EncryptBlockAvx2(input, keyVector);
				output.StoreUnsafe(ref MemoryMarshal.GetReference(vectorizedResult[offset..]));
			}

			// process the remaining input
			if (totalVectorizedLength < text.Length)
			{
				var remainingInput = text[totalVectorizedLength..];
				var remainingOutput = result[totalVectorizedLength..];
				EncryptSlow(remainingInput, remainingOutput, key);
			}
		}
		else
		{
			EncryptSlow(text, result, key);
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

		if (Avx2.IsSupported)
		{
			// process the vectorized input
			var vectorCount = text.Length / Vector128<short>.Count;
			var totalVectorizedLength = vectorCount * Vector128<short>.Count;
			var vectorizedText = MemoryMarshal.Cast<char, short>(text);
			var vectorizedResult = MemoryMarshal.Cast<char, short>(result);
			var keyVector = Vector128.Create((short)key);
			for (int offset = 0; offset < totalVectorizedLength; offset += Vector128<short>.Count)
			{
				var input = Unsafe.As<short, Vector128<short>>(ref MemoryMarshal.GetReference(vectorizedText[offset..]));
				var output = DecryptBlockAvx2(input, keyVector);
				output.StoreUnsafe(ref MemoryMarshal.GetReference(vectorizedResult[offset..]));
			}

			// process the remaining input
			if (totalVectorizedLength < text.Length)
			{
				var remainingInput = text[totalVectorizedLength..];
				var remainingOutput = result[totalVectorizedLength..];
				DecryptSlow(remainingInput, remainingOutput, key);
			}
		}
		else
		{
			DecryptSlow(text, result, key);
		}

		written = text.Length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<short> EncryptBlockAvx2(Vector128<short> input, Vector128<short> keyVector)
	{
		// uppercase
		var isUpperMask = Avx2.And(
			Avx2.CompareGreaterThan(input, VectorOfAMinus1),
			Avx2.CompareLessThan(input, VectorOfZPlus1)
		);
		var transformedUppercase = Avx2.Add(input, keyVector);

		// normalize overflow
		var uppercaseOverflowMask = Avx2.And(Avx2.CompareGreaterThan(transformedUppercase, VectorOfZ), isUpperMask);
		var uppercaseOverflow = Avx2.Subtract(transformedUppercase, VectorOfZPlus1);
		var uppercaseNormalized = Avx2.Add(VectorOfA, uppercaseOverflow);
		transformedUppercase = Avx2.BlendVariable(transformedUppercase, uppercaseNormalized, uppercaseOverflowMask);
		transformedUppercase = Avx2.BlendVariable(input, transformedUppercase, isUpperMask);

		// lowercase
		var isLowerMask = Avx2.And(
			Avx2.CompareGreaterThan(input, VectorOfLowercaseAMinus1),
			Avx2.CompareLessThan(input, VectorOfLowercaseZPlus1)
		);
		var transformedLowercase = Avx2.Add(input, keyVector);

		// normalize overflow
		var lowercaseOverflowMask = Avx2.And(Avx2.CompareGreaterThan(transformedLowercase, VectorOfLowercaseZ), isLowerMask);
		var lowercaseOverflow = Avx2.Subtract(transformedLowercase, VectorOfLowercaseZPlus1);
		var lowercaseNormalized = Avx2.Add(VectorOfLowercaseA, lowercaseOverflow);
		transformedLowercase = Avx2.BlendVariable(transformedLowercase, lowercaseNormalized, lowercaseOverflowMask);
		transformedLowercase = Avx2.BlendVariable(transformedLowercase, transformedLowercase, isLowerMask);

		// merge
		var transformed = Avx2.BlendVariable(transformedUppercase, transformedLowercase, isLowerMask);

		return transformed;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<short> DecryptBlockAvx2(Vector128<short> input, Vector128<short> keyVector)
	{
		// uppercase
		var isUpperMask = Avx2.And(
			Avx2.CompareGreaterThan(input, VectorOfAMinus1),
			Avx2.CompareLessThan(input, VectorOfZPlus1)
		);
		var transformedUppercase = Avx2.Subtract(input, keyVector);

		// normalize underflow
		var uppercaseUnderflowMask = Avx2.And(Avx2.CompareLessThan(transformedUppercase, VectorOfA), isUpperMask);
		var uppercaseUnderflow = Avx2.Subtract(VectorOfA, transformedUppercase);
		var uppercaseNormalized = Avx2.Subtract(VectorOfZPlus1, uppercaseUnderflow);
		transformedUppercase = Avx2.BlendVariable(transformedUppercase, uppercaseNormalized, uppercaseUnderflowMask);
		transformedUppercase = Avx2.BlendVariable(input, transformedUppercase, isUpperMask);

		// lowercase
		var isLowerMask = Avx2.And(
			Avx2.CompareGreaterThan(input, VectorOfLowercaseAMinus1),
			Avx2.CompareLessThan(input, VectorOfLowercaseZPlus1)
		);
		var transformedLowercase = Avx2.Subtract(input, keyVector);

		// normalize underflow
		var lowercaseUnderflowMask = Avx2.And(Avx2.CompareLessThan(transformedLowercase, VectorOfLowercaseA), isLowerMask);
		var lowercaseUnderflow = Avx2.Subtract(VectorOfLowercaseA, transformedLowercase);
		var lowercaseNormalized = Avx2.Subtract(VectorOfLowercaseZPlus1, lowercaseUnderflow);
		transformedLowercase = Avx2.BlendVariable(transformedLowercase, lowercaseNormalized, lowercaseUnderflowMask);
		transformedLowercase = Avx2.BlendVariable(transformedLowercase, transformedLowercase, isLowerMask);

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

				var value = (char)(ch + key);

				// normalize overflow
				if (value > 'z')
				{
					value -= (char)AlphabetLength;
				}

				result[i] = value;
			}

			// uppercase
			else if (ch >= 'A')
			{
				if (ch > 'Z')
				{
					result[i] = ch;
					continue;
				}

				var value = (char)(ch + key);

				// normalize overflow
				if (value > 'Z')
				{
					value -= (char)AlphabetLength;
				}

				result[i] = value;
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

				var value = (char)(ch - key);

				// normalize underflow
				if (value < 'A')
				{
					value += (char)AlphabetLength;
				}

				result[i] = value;
			}

			// uppercase
			else if (ch <= 'z')
			{
				if (ch < 'a')
				{
					result[i] = ch;
					continue;
				}

				var value = (char)(ch - key);

				// normalize underflow
				if (value < 'a')
				{
					value += (char)AlphabetLength;
				}

				result[i] = value;
			}

			// non-alphabet
			else
			{
				result[i] = ch;
			}
		}
	}
}
