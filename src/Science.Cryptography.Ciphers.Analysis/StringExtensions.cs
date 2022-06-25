using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

internal static class StringExtensions
{
	public static IEnumerable<int> AllIndexesOf(this StringSegment source, StringSegment subject, int startIndex = 0, StringComparison comparison = StringComparison.Ordinal)
	{
		int idx = source.AsSpan()[startIndex..].IndexOf(subject.AsSpan(), comparison);

		while (idx != -1)
		{
			yield return idx;
			var nextStart = idx + 1;
			if (nextStart >= source.Length)
			{
				break;
			}

			idx = source.AsSpan()[nextStart..].IndexOf(subject.AsSpan(), comparison);
		}
	}

	public static AllIndexesEnumerable AllIndexesOf(this ReadOnlySpan<char> source, ReadOnlySpan<char> subject, int startIndex = 0, StringComparison comparison = StringComparison.Ordinal) =>
		new(source, subject, startIndex, comparison);


	public ref struct AllIndexesEnumerable
	{
		public AllIndexesEnumerable(ReadOnlySpan<char> source, ReadOnlySpan<char> subject, int startIndex, StringComparison comparison)
		{
			_source = source;
			_subject = subject;
			_startIndex = startIndex;
			_comparison = comparison;
		}

		private readonly ReadOnlySpan<char> _source;
		private readonly ReadOnlySpan<char> _subject;
		private readonly int _startIndex;
		private readonly StringComparison _comparison;

		public AllIndexesEnumerator GetEnumerator() => new(_source, _subject, _startIndex, _comparison);

		public ref struct AllIndexesEnumerator
		{
			public AllIndexesEnumerator(ReadOnlySpan<char> source, ReadOnlySpan<char> subject, int startIndex, StringComparison comparison)
			{
				_source = source;
				_subject = subject;
				_startIndex = startIndex;
				_comparison = comparison;
			}

			private readonly ReadOnlySpan<char> _source;
			private readonly ReadOnlySpan<char> _subject;
			private readonly int _startIndex;
			private readonly StringComparison _comparison;

			private int _current = -1;

			public int Current => _current;

			public bool MoveNext()
			{
				var startIndex = _current + 1;
				if (startIndex >= _source.Length)
				{
					return false;
				}

				_current = _source[startIndex..].IndexOf(_subject, _comparison);
				return _current > -1;
			}

			public void Reset()
			{
				_current = _startIndex - 1;
			}
		}
	}
}
