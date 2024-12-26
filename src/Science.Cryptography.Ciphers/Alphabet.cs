using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers;

public class Alphabet : IReadOnlyList<char>, IEquatable<Alphabet>
{
	public Alphabet(ReadOnlySpan<char> characters)
	{
		// build index lookup
		var lookup = new Dictionary<char, int>(characters.Length);
		for (int i = 0; i < characters.Length; i++)
		{
			lookup.Add(characters[i], i);
		}
		_indexLookup = lookup.ToFrozenDictionary();

		// build index lookup (case-insensitive)
		var lookupUpper = new Dictionary<char, int>(characters.Length);
		for (int i = 0; i < characters.Length; i++)
		{
			lookupUpper.Add(characters[i].ToUpperInvariant(), i);
		}
		_indexLookupUpper = lookupUpper.ToFrozenDictionary();

		_chars = characters.ToArray();
		_str = new string(characters);
		DoubleLength = characters.Length * 2;
		MinusLength = -characters.Length;
	}

	private readonly char[] _chars;
	private readonly string _str;
	private readonly FrozenDictionary<char, int> _indexLookup;
	private readonly FrozenDictionary<char, int> _indexLookupUpper;
	private readonly int DoubleLength;
	private readonly int MinusLength;

	public static Alphabet FromKeyword(string keyword, Alphabet @base, bool throwOnDuplicates = false)
	{
		var buffer = new char[@base.Length];
		ArrayHelper.FillWithKeywordAndAlphabet(buffer, keyword, @base.AsSpan(), throwOnDuplicates);
		return new(buffer);
	}

	public static Alphabet FromKeyword(ReadOnlySpan<char> keyword, Alphabet @base, bool throwOnDuplicates = false)
	{
		var buffer = new char[@base.Length];
		ArrayHelper.FillWithKeywordAndAlphabet(buffer, keyword, @base.AsSpan(), throwOnDuplicates);
		return new(buffer);
	}


	public char this[int index] => _chars[index];

	public char this[Index index] => _chars[index];
	public ReadOnlySpan<char> this[Range index]
	{
		get
		{
			var offset = index.GetOffsetAndLength(_str.Length);
			return _str.AsSpan(offset.Offset, offset.Length);
		}
	}

	public int Length => _chars.Length;
	int IReadOnlyCollection<char>.Count => Length;

	public bool Contains(char c) => _indexLookup.ContainsKey(c);

	public bool Contains(char c, IEqualityComparer<char> comparer) => IndexOf(c, comparer) != -1;

	public bool Contains(char c, StringComparison comparison) => _str.Contains(c, comparison);

	public bool ContainsIgnoreCase(char c) => _indexLookupUpper.ContainsKey(c.ToUpperInvariant());

	public int IndexOf(char c) => _indexLookup.TryGetValue(c, out var index) ? index : -1;

	public int IndexOf(char c, StringComparison comparison) => _str.IndexOf(c, comparison);

	public int IndexOf(char c, IEqualityComparer<char> comparer)
	{
		unchecked
		{
			for (int i = 0; i < Length; i++)
			{
				if (comparer.Equals(c, _chars[i]))
				{
					return i;
				}
			}

			return -1;
		}
	}

	public int IndexOfIgnoreCase(char c) => _indexLookupUpper.TryGetValue(c.ToUpperInvariant(), out var index) ? index : -1;

	public char AtMod(int index)
	{
		// negative
		if (index < 0)
		{
			// spare modulo operation
			if (index > MinusLength)
			{
				index = index - MinusLength;
			}

			else
			{
				index = (Length - (-index % Length)) % Length;
			}
		}

		// overflow
		else if (index >= Length)
		{
			// spare modulo operation
			if (index < DoubleLength)
			{
				index = index - Length;
			}

			else
			{
				index = index % Length;
			}
		}

		return _chars[index];
	}

	public override bool Equals(object obj) => Equals(obj as Alphabet);

	public bool Equals(Alphabet other) => other != null && _chars.AsSpan().SequenceEqual(other._chars);

	public override int GetHashCode() => HashCode.Combine(_chars);

	public static bool operator ==(Alphabet left, Alphabet right) => Object.ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

	public static bool operator !=(Alphabet left, Alphabet right) => !(left == right);

	public static implicit operator Alphabet(string s) => new(s);


	IEnumerator<char> IEnumerable<char>.GetEnumerator() => ((IEnumerable<char>)_chars).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => _chars.GetEnumerator();


	public override string ToString() => _str;

	public char[] ToCharArray()
	{
		var copy = new char[_chars.Length];
		_chars.CopyTo(copy, 0);
		return copy;
	}

	public ReadOnlyMemory<char> ToMemory() => _str.AsMemory();

	public PolybiusSquare ToPolybiusSquare() => PolybiusSquare.CreateFromAlphabet(this);

	public void CopyTo(Span<char> destination) => _chars.CopyTo(destination);

	public ReadOnlySpan<char> AsSpan() => _chars;
}
