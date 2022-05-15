using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers;

public class Alphabet : IReadOnlyList<char>, IEquatable<Alphabet>
{
	public Alphabet(IEnumerable<char> characters)
	{
		_chars = characters.Distinct().Select(Char.ToUpperInvariant).ToArray();
		_str = new string(_chars);
	}

	private readonly char[] _chars;
	private readonly string _str;

	public static Alphabet FromKeyword(string keyword, Alphabet @base, bool throwOnDuplicates = false)
	{
		var buffer = new char[@base.Length];
		int written = 0;

		for (int i = 0; i < keyword.Length; i++)
		{
			var upper = keyword[i].ToUpper();
			if (Array.IndexOf(buffer, upper) != -1)
			{
				buffer[written++] = upper;
			}
		}

		if (throwOnDuplicates && written < keyword.Length)
		{
			throw new ArgumentOutOfRangeException(nameof(keyword), "Keyword contains duplicate characters.");
		}

		for (int i = 0; i < @base.Length; i++)
		{
			var upper = @base[i].ToUpper();
			if (Array.IndexOf(buffer, upper) != -1)
			{
				buffer[written++] = upper;
			}
		}

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

	public bool Contains(char c) => _chars.Contains(c);

	public bool Contains(char c, IEqualityComparer<char> comparer) => IndexOf(c, comparer) != -1;

	public bool Contains(char c, StringComparison comparison) => _str.Contains(c, comparison);

	public int IndexOf(char c) => _str.IndexOf(c);

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

	public int IndexOfIgnoreCase(char c) => _str.IndexOf(c, StringComparison.OrdinalIgnoreCase);

	public char AtMod(int index) => _chars[index switch
	{
		< 0 => (Length - (-index % Length)) % Length,
		_ => index % Length,
	}];


	public override bool Equals(object obj) => Equals(obj as Alphabet);

	public bool Equals(Alphabet other) => other != null && _chars.AsSpan().SequenceEqual(other._chars);

	public override int GetHashCode() => HashCode.Combine(_chars);

	public static bool operator ==(Alphabet left, Alphabet right) => Object.ReferenceEquals(left, right) || (left?.Equals(right) ?? false);

	public static bool operator !=(Alphabet left, Alphabet right) => !(left == right);

	public static implicit operator Alphabet(string s) => new(s);


	IEnumerator<char> IEnumerable<char>.GetEnumerator() => ((IEnumerable<char>)_chars).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => _chars.GetEnumerator();


	public override string ToString() => _str;

	public char[] ToCharArray() => _chars.ToArray();

	public PolybiusSquare ToPolybiusSquare() => PolybiusSquare.CreateFromAlphabet(this);

	public void CopyTo(Span<char> destination) => _chars.CopyTo(destination);

	public ReadOnlySpan<char> AsSpan() => _chars;
}
