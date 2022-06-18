using System;
using System.Numerics;
using System.Reflection;

namespace Science.Cryptography.Ciphers.Analysis;

internal ref struct ValueBitArray
{
	public ValueBitArray(Span<ulong> buffer)
	{
		_buffer = buffer;
	}

	private readonly Span<ulong> _buffer;
	private const int Size = sizeof(ulong) * 8;

	public bool this[int index]
	{
		get
		{
			(int bucket, int position) = Math.DivRem(index, Size);
			return (_buffer[bucket] & (1ul << position)) > 0ul;
		}
		set
		{
			(int bucket, int position) = Math.DivRem(index, Size);
			_buffer[bucket] = value switch
			{
				true => _buffer[bucket] | (1ul << position),
				false => _buffer[bucket] & ~(1ul << position),
			};
		}
	}

	public void Reset()
	{
		_buffer.Fill(0ul);
	}

	public int GetCardinality()
	{
		int count = 0;
		for (int i = 0; i < _buffer.Length; i++)
		{
			count += BitOperations.PopCount(_buffer[i]);
		}
		return count;
	}

	public void SetRangeToOne(int offset, int length)
	{
		(int firstBucket, int positionInFirstBucket) = Math.DivRem(offset, Size);
		(int lastBucket, int positionInLastBucket) = Math.DivRem(offset + length, Size);

		if (firstBucket == lastBucket)
		{
			_buffer[firstBucket] |= ulong.MaxValue >> (Size - length) << offset;
		}
		else
		{
			throw new NotImplementedException();
		}
	}

	public static int GetNumberOfBucketsForBits(int bits) => bits / Size + 1;
}
