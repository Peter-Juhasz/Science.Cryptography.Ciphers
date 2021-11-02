using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers;

public class Alphabet : IReadOnlyCollection<char>, IEquatable<Alphabet>
{
    public Alphabet(IEnumerable<char> characters)
    {
        _chars = characters.ToArray();
        _str = new string(_chars);
    }

    private readonly char[] _chars;
    private readonly string _str;


    public static Alphabet FromKeyword(string keyword, Alphabet @base)
    {
        throw new NotImplementedException();
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

    public int IndexOf(char c) => Array.IndexOf(_chars, c);

    public int IndexOfIgnoreCase(char c) => _str.IndexOf(c, StringComparison.OrdinalIgnoreCase);

    public char AtMod(int index) => _chars[index.Mod(Length)];


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
}
