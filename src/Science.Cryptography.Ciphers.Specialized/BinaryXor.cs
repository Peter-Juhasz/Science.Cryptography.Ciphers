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
            BinaryXor.Avx2Xor256(input, output, key);
        }
        else
        {
            BinaryXor.SlowXor(input, output, key);
        }
    }

    public static unsafe void Avx2Xor256(ReadOnlySpan<byte> input, Span<byte> output, ReadOnlySpan<byte> key)
    {
        // initialize
        var length = input.Length;
        int keyLength = key.Length;
        int vectorLength = Vector256<byte>.Count, maxVectorLength = length / vectorLength * vectorLength;
        Span<byte> keyVectorBytes = stackalloc byte[vectorLength];

        // chunks
        for (int chunkStartIndex = 0; chunkStartIndex < maxVectorLength; chunkStartIndex += vectorLength)
        {
            // input span as vector
            ref byte pointer = ref MemoryMarshal.GetReference(input[chunkStartIndex..(chunkStartIndex + vectorLength)]);
            var vector = Unsafe.As<byte, Vector256<byte>>(ref pointer);

            // fill vector with running instances of key
            var rem = chunkStartIndex % keyLength;
            if (rem > 0)
            {
                key[(keyLength - rem)..].CopyTo(keyVectorBytes);
            }

            var nextChunkStartIndex = chunkStartIndex + vectorLength;
            int keyCopyIndex;
            for (keyCopyIndex = chunkStartIndex + rem; keyCopyIndex + keyLength <= nextChunkStartIndex; keyCopyIndex += keyLength)
            {
                key.CopyTo(keyVectorBytes[(keyCopyIndex - chunkStartIndex)..]);
            }

            var remainingKeyLength = nextChunkStartIndex - keyCopyIndex;
            if (remainingKeyLength > 0)
            {
                key[..remainingKeyLength].CopyTo(keyVectorBytes[keyCopyIndex..]);
            }

            ref byte keyPointer = ref MemoryMarshal.GetReference(keyVectorBytes);
            var keyVector = Unsafe.As<byte, Vector256<byte>>(ref keyPointer);

            // execute xor
            var result = Avx2.Xor(vector, keyVector);

            // read from vector
            fixed (byte* outputPointer = &MemoryMarshal.GetReference(output[chunkStartIndex..]))
            {
                Avx.Store(outputPointer, result);
            }
        }

        // remaining
        if (length != maxVectorLength)
        {
            var remaining = length - maxVectorLength;
            SlowXor(input[^remaining..], output[^remaining..], key, keyStart: maxVectorLength);
        }
    }

    public static void SlowXor(ReadOnlySpan<byte> input, Span<byte> output, ReadOnlySpan<byte> key, int keyStart = 0)
    {
        for (int i = 0; i < input.Length; i++)
        {
            output[i] = (byte)(input[i] ^ key[(keyStart + i) % key.Length]);
        }
    }
}
