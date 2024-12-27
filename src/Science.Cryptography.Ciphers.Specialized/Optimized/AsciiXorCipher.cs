using System;
using System.Composition;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Science.Cryptography.Ciphers.Specialized;

/// <summary>
/// Represents the XOR cipher.
/// </summary>
[Export("ASCII-XOR", typeof(IKeyedCipher<>))]
public class AsciiXorCipher : ReciprocalKeyedCipher<byte[]>
{
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

		// prepare key
		Span<ushort> ushortKey = stackalloc ushort[key.Length];
		for (int i = 0; i < key.Length; i++)
		{
			ushortKey[i] = key[i];
		}

		// crypt
		var inputBytes = MemoryMarshal.Cast<char, ushort>(input);
		var outputBytes = MemoryMarshal.Cast<char, ushort>(output);
		Xor(inputBytes, outputBytes, ushortKey);

		// decode
		written = input.Length;
	}

	public static void Xor(ReadOnlySpan<ushort> input, Span<ushort> output, ReadOnlySpan<ushort> key)
	{
		if (output.Length < input.Length)
		{
			throw new ArgumentException("Size of output buffer is insufficient.", nameof(output));
		}

		if (Avx2.IsSupported)
		{
			if (input.Length < 256 / 8)
			{
				SlowXor(input, output, key);
			}
			else
			{
				Avx2Xor256(input, output, key);
			}
		}
		else
		{
			SlowXor(input, output, key);
		}
	}

	public static void Avx2Xor256(ReadOnlySpan<ushort> input, Span<ushort> output, ReadOnlySpan<ushort> key)
	{
		// initialize
		var length = input.Length;
		int keyLength = key.Length;
		int vectorLength = Vector256<ushort>.Count, maxVectorLength = length / vectorLength * vectorLength;

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
		Span<ushort> keyVectorUnits = stackalloc ushort[isVectorLengthMultipleOfKeyLength switch
		{
			true => vectorLength,
			false => keyLength + vectorLength - 1,
		}];
		if (isVectorLengthMultipleOfKeyLength)
		{
			for (int keyCopyIndex = 0; keyCopyIndex < keyVectorUnits.Length; keyCopyIndex += keyLength)
			{
				key.CopyTo(keyVectorUnits[keyCopyIndex..]);
			}
		}
		else if (isKeyLongerThanVector)
		{
			key.CopyTo(keyVectorUnits);
			key[..(vectorLength - 1)].CopyTo(keyVectorUnits[keyLength..]);
		}
		else
		{
			var remaining = keyVectorUnits.Length % keyLength;
			var lastPosition = keyVectorUnits.Length - remaining;
			int keyCopyIndex;
			for (keyCopyIndex = 0; keyCopyIndex < lastPosition; keyCopyIndex += keyLength)
			{
				key.CopyTo(keyVectorUnits[keyCopyIndex..]);
			}
			key[..remaining].CopyTo(keyVectorUnits[keyCopyIndex..]);
		}

		// chunks of vector size
		Vector256<ushort> keyVector;
		Unsafe.SkipInit(out keyVector);
		for (int chunkStartIndex = 0; chunkStartIndex < maxVectorLength; chunkStartIndex += vectorLength)
		{
			// input span as vector
			ref ushort pointer = ref MemoryMarshal.GetReference(input[chunkStartIndex..(chunkStartIndex + vectorLength)]);
			var vector = Unsafe.As<ushort, Vector256<ushort>>(ref pointer);

			// select the matching slice of the key ushorts buffer
			if (isVectorLengthMultipleOfKeyLength)
			{
				if (chunkStartIndex == 0)
				{
					ref ushort keyPointer = ref MemoryMarshal.GetReference(keyVectorUnits);
					keyVector = Unsafe.As<ushort, Vector256<ushort>>(ref keyPointer);
				}
			}
			else
			{
				ref ushort keyPointer = ref MemoryMarshal.GetReference(keyVectorUnits[(chunkStartIndex % keyLength)..]);
				keyVector = Unsafe.As<ushort, Vector256<ushort>>(ref keyPointer);
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

	public static void SlowXor(ReadOnlySpan<ushort> input, Span<ushort> output, ReadOnlySpan<ushort> key, int keyStart = 0)
	{
		int inputLength = input.Length;
		int keyLength = key.Length;
		for (int i = 0; i < inputLength; i++)
		{
			output[i] = (ushort)(input[i] ^ key[(keyStart + i) % keyLength]);
		}
	}
}
