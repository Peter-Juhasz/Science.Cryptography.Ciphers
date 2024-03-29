using Microsoft.Extensions.Primitives;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Contains n-gram analysis tools.
/// </summary>
public static partial class NGramAnalysis
{
	/// <summary>
	/// Converts a given <paramref name="text"/> to <paramref name="length"/>-length sequences (including overlapping ones).
	/// </summary>
	/// <param name="text">The text to convert.</param>
	/// <param name="length">The length of the sequences.</param>
	/// <returns>All <paramref name="length"/>-grams.</returns>
	/// <example>ABCD to 2-grams: AB, BC, CD</example>
	public static NGramStringSegmentEnumerable ReadNGrams(this string text, int length)
	{
		if (length <= 0)
			throw new ArgumentOutOfRangeException(nameof(length));

		return new(text, length);
	}

	/// <summary>
	/// Measures <paramref name="length"/>-gram frequencies in a given text.
	/// </summary>
	/// <param name="text">Text to analyze.</param>
	/// <param name="length">The length of n-gram sequences.</param>
	/// <returns>Frequencies of <paramref name="length"/>-grams in the text.</returns>
	public static AbsoluteStringFrequencies Analyze(string text, int length, IEqualityComparer<string> comparer)
	{
		var result = new Dictionary<string, int>(capacity: text.Length - length + 1, comparer);

		foreach (var segment in ReadNGrams(text, length))
		{
			var str = segment.ToString();
			ref int frequency = ref CollectionsMarshal.GetValueRefOrAddDefault(result, str, out _);
			frequency++;
		}

		return new(result);
	}

	/// <summary>
	/// Measures <paramref name="length"/>-gram frequencies in a given text.
	/// </summary>
	/// <param name="text">Text to analyze.</param>
	/// <param name="length">The length of n-gram sequences.</param>
	/// <returns>Frequencies of <paramref name="length"/>-grams in the text.</returns>
	public static AbsoluteStringFrequencies AnalyzeLetters(string text, int length)
	{
		var result = new Dictionary<string, int>(capacity: text.Length - length + 1, StringComparer.OrdinalIgnoreCase);

		foreach (var segment in ReadNGrams(text, length))
		{
			var onlyLetters = true;
			foreach (var ch in segment.AsSpan())
			{
				if (!Char.IsLetter(ch))
				{
					onlyLetters = false;
					break;
				}
			}

			if (!onlyLetters)
			{
				continue;
			}

			var str = segment.ToString(); 
			ref int frequency = ref CollectionsMarshal.GetValueRefOrAddDefault(result, str, out _);
			frequency++;
		}

		return new(result);
	}


	public readonly struct NGramStringSegmentEnumerable : IEnumerable<StringSegment>
	{
		internal NGramStringSegmentEnumerable(string text, int length)
		{
			_text = text;
			_length = length;
		}

		private readonly string _text;
		private readonly int _length;

		public NGramEnumerator GetEnumerator() => new(_text, _length);

		IEnumerator<StringSegment> IEnumerable<StringSegment>.GetEnumerator() => GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public struct NGramEnumerator : IEnumerator<StringSegment>
		{
			internal NGramEnumerator(string text, int length)
			{
				_text = text;
				_length = length;
				_textLength = text.Length;
				_offset = -1;
			}

			private readonly string _text;
			private readonly int _length;
			private readonly int _textLength;
			private int _offset;

			public StringSegment Current => new(_text, _offset, _length);

			object IEnumerator.Current => Current;

			public bool MoveNext()
			{
				_offset++;
				return _offset + _length <= _textLength;
			}

			public void Reset()
			{
				_offset = -1;
			}

			public void Dispose() { }
		}
	}
}
