using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Science.Cryptography.Ciphers.Analysis;

public sealed class ArrayKeySpace<T> : IPartitionedKeySpace<T[]> where T : IEquatable<T>
{
	public ArrayKeySpace(int minimumLength, int maximumLength, IReadOnlySet<T> values, T[]? startFrom = null)
	{
		if (minimumLength < 1)
		{
			throw new ArgumentOutOfRangeException(nameof(minimumLength));
		}

		if (maximumLength < 1)
		{
			throw new ArgumentOutOfRangeException(nameof(maximumLength));
		}

		if (maximumLength < minimumLength)
		{
			throw new ArgumentOutOfRangeException(nameof(maximumLength));
		}

		if (values.Count == 0)
		{
			throw new ArgumentOutOfRangeException(nameof(values));
		}

		if (startFrom != null)
		{
			if (startFrom.Length != minimumLength)
			{
				throw new ArgumentOutOfRangeException(nameof(startFrom));
			}

			if (startFrom.Any(v => !values.Contains(v)))
			{
				throw new ArgumentOutOfRangeException(nameof(startFrom));
			}
		}

		MinimumLength = minimumLength;
		MaximumLength = maximumLength;
		Values = values.ToArray();
		StartFrom = startFrom;
	}

	public int MinimumLength { get; }
	public int MaximumLength { get; }
	public T[] Values { get; }
	public T[]? StartFrom { get; }

	public IEnumerable<IKeySpace<T[]>> GetPartitions(int? desiredCount = 0)
	{
		// TODO: better distribution by calculating with desiredCount

		for (int length = MinimumLength; length <= MaximumLength; length++)
		{
			T[] startFrom = new T[length];
			var startValueIndex = 0;

			if (length == MinimumLength && StartFrom != null)
			{
				yield return new ArrayKeySpacePartition(length, Values, StartFrom);
				startValueIndex = Array.IndexOf(Values, StartFrom[0]) + 1;
			}

			for (int i = startValueIndex; i < Values.Length; i++)
			{
				startFrom[0] = Values[i];
				startFrom.AsSpan(1).Fill(Values[0]);

				yield return new ArrayKeySpacePartition(length, Values, startFrom);
			}
		}
	}

	private sealed class ArrayKeySpacePartition : IKeySpace<T[]>
	{
		public ArrayKeySpacePartition(int length, T[] values, T[]? startFrom = null)
		{
			_buffer = new T[length];
			if (startFrom == null)
			{
				_buffer.AsSpan().Fill(values[0]);
			}
			else
			{
				startFrom.AsSpan().CopyTo(_buffer);
				_prependWithStartFrom = true;
			}
			_lastIndex = length - 1;
			_lastValue = values[^1];
			_values = values;
		}

		private readonly T[] _buffer;
		private readonly int _lastIndex;
		private readonly T _lastValue;
		private readonly T[] _values;
		private readonly bool _prependWithStartFrom = false;

		public IEnumerable<T[]> GetKeys()
		{
			if (_prependWithStartFrom)
			{
				yield return _buffer;
			}

			while (TryAdvance())
			{
				yield return _buffer;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		private bool TryAdvance()
		{
			for (int i = _lastIndex; i > 0; i--)
			{
				if (!_buffer[i].Equals(_lastValue))
				{
					var index = Array.IndexOf(_values, _buffer[i]);
					_buffer[i] = _values[index + 1];

					if (i + 1 <= _lastIndex)
					{
						_buffer.AsSpan(i + 1).Fill(_values[0]);
					}

					return true;
				}
			}

			return false;
		}
	}
}