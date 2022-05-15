using System.Collections;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers;

public static partial class ValueEnumerableExtensions
{
	public static ReadOnlyListEnumerable<T> AsValueEnumerable<T>(this IReadOnlyList<T> source) => new(source);

	public static ListEnumerable<T> AsValueEnumerable<T>(this IList<T> source) => new(source);

	public readonly struct ReadOnlyListEnumerable<T> : IEnumerable<T>
	{
		public ReadOnlyListEnumerable(IReadOnlyList<T> source)
		{
			this.source = source;
		}

		private readonly IReadOnlyList<T> source;

		public Enumerator GetEnumerator() => new(source);

		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();


		public struct Enumerator : IEnumerator<T>
		{
			public Enumerator(IReadOnlyList<T> source)
			{
				Current = default!;
				enumerator = source;
				index = -1;
			}

			private readonly IReadOnlyList<T> enumerator;

			public T Current { get; private set; }

			object? IEnumerator.Current => this.Current;

			private int index;

			public bool MoveNext()
			{
				if (++index < enumerator.Count)
				{
					Current = enumerator[index];
					return true;
				}
				else
				{
					return false;
				}
			}

			public void Reset()
			{
				index = -1;
			}

			public void Dispose() { }
		}
	}

	public readonly struct ListEnumerable<T> : IEnumerable<T>
	{
		public ListEnumerable(IList<T> source)
		{
			this.source = source;
		}

		private readonly IList<T> source;

		public Enumerator GetEnumerator() => new(source);

		IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

		IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();


		public struct Enumerator : IEnumerator<T>
		{
			public Enumerator(IList<T> source)
			{
				Current = default!;
				enumerator = source;
				index = -1;
			}

			private readonly IList<T> enumerator;

			public T Current { get; private set; }

			object? IEnumerator.Current => this.Current;

			private int index;

			public bool MoveNext()
			{
				if (++index < enumerator.Count)
				{
					Current = enumerator[index];
					return true;
				}
				else
				{
					return false;
				}
			}

			public void Reset()
			{
				index = -1;
			}

			public void Dispose() { }
		}
	}
}
