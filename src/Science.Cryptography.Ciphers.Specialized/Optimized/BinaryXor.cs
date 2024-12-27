using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Science.Cryptography.Ciphers.Specialized;

public static class BinaryXor
{
	public static void Xor(ReadOnlySpan<byte> input, Span<byte> output, ReadOnlySpan<byte> key)
	{
		if (output.Length < input.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(output));
		}

		if (Avx2.IsSupported)
		{
			if (input.Length < 256 / 8)
			{
				BinaryXor.SlowXor(input, output, key);
			}
			else
			{
				BinaryXor.Avx2Xor256(input, output, key);
			}
		}
		else
		{
			BinaryXor.SlowXor(input, output, key);
		}
	}

	public static void Avx2Xor256(ReadOnlySpan<byte> input, Span<byte> output, ReadOnlySpan<byte> key)
	{
		// initialize
		var length = input.Length;
		int keyLength = key.Length;
		int vectorLength = Vector256<byte>.Count, maxVectorLength = length / vectorLength * vectorLength;

		// prefill buffer with running instances of key,
		// so the right slice can be instantly selected
		//
		// same length:
		// vector (4): 01 02 03 04
		// key (4):    01 02 03 04
		//
		// key is longer:
		// vector (4): 01 02 03 04
		// key (6):    01 02 03 04 05 06 01 02 03
		// 
		// key is shorter:
		// vector (4): 01 02 03 04
		// key (3):    01 02 03 01 02 03
		bool isVectorLengthMultipleOfKeyLength = (vectorLength % keyLength) == 0;
		bool isKeyLongerThanVector = vectorLength < keyLength;
		Span<byte> keyVectorBytes = stackalloc byte[isVectorLengthMultipleOfKeyLength switch
		{
			true => vectorLength,
			false => keyLength + vectorLength - 1,
		}];
		if (isVectorLengthMultipleOfKeyLength)
		{
			for (int keyCopyIndex = 0; keyCopyIndex < keyVectorBytes.Length; keyCopyIndex += keyLength)
			{
				key.CopyTo(keyVectorBytes[keyCopyIndex..]);
			}
		}
		else if (isKeyLongerThanVector)
		{
			key.CopyTo(keyVectorBytes);
			key[..(vectorLength - 1)].CopyTo(keyVectorBytes[keyLength..]);
		}
		else
		{
			var remaining = keyVectorBytes.Length % keyLength;
			var lastPosition = keyVectorBytes.Length - remaining;
			int keyCopyIndex;
			for (keyCopyIndex = 0; keyCopyIndex < lastPosition; keyCopyIndex += keyLength)
			{
				key.CopyTo(keyVectorBytes[keyCopyIndex..]);
			}
			key[..remaining].CopyTo(keyVectorBytes[keyCopyIndex..]);
		}

		// chunks of vector size
		Vector256<byte> keyVector;
		Unsafe.SkipInit(out keyVector);
		for (int chunkStartIndex = 0; chunkStartIndex < maxVectorLength; chunkStartIndex += vectorLength)
		{
			// input span as vector
			ref byte pointer = ref MemoryMarshal.GetReference(input[chunkStartIndex..(chunkStartIndex + vectorLength)]);
			var vector = Unsafe.As<byte, Vector256<byte>>(ref pointer);

			// select the matching slice of the key bytes buffer
			if (isVectorLengthMultipleOfKeyLength)
			{
				if (chunkStartIndex == 0)
				{
					ref byte keyPointer = ref MemoryMarshal.GetReference(keyVectorBytes);
					keyVector = Unsafe.As<byte, Vector256<byte>>(ref keyPointer);
				}
			}
			else
			{
				ref byte keyPointer = ref MemoryMarshal.GetReference(keyVectorBytes[(chunkStartIndex % keyLength)..]);
				keyVector = Unsafe.As<byte, Vector256<byte>>(ref keyPointer);
			}

			// execute xor
			var result = Avx2.Xor(vector, keyVector);

			// read from vector
			result.StoreUnsafe(ref MemoryMarshal.GetReference(output[chunkStartIndex..]));
		}

		// process final block
		if (length != maxVectorLength)
		{
			var remaining = length - maxVectorLength;
			SlowXor(input[^remaining..], output[^remaining..], key, keyStart: maxVectorLength);
		}
	}

	public static void SlowXor(ReadOnlySpan<byte> input, Span<byte> output, ReadOnlySpan<byte> key, int keyStart = 0)
	{
		int inputLength = input.Length;
		int keyLength = key.Length;
		for (int i = 0; i < inputLength; i++)
		{
			output[i] = (byte)(input[i] ^ key[(keyStart + i) % keyLength]);
		}
	}
}
