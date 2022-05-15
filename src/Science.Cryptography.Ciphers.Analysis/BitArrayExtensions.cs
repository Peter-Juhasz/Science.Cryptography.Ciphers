using System.Buffers;
using System.Collections;
using System.Numerics;

namespace Science.Cryptography.Ciphers.Analysis;

internal static class BitArrayExtensions
{ 
	public static int GetCardinality(this BitArray bitArray)
	{
		var length = (bitArray.Count >> 5) + 1;
		uint[] ints = ArrayPool<uint>.Shared.Rent(length);
		bitArray.CopyTo(ints, 0);
		int count = 0;
		for (int i = 0; i < ints.Length; i++)
		{
			count += BitOperations.PopCount(ints[i]);
		}
		ArrayPool<uint>.Shared.Return(ints);
		return count;
	}
}
