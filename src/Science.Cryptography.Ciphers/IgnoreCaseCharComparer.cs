using System.Collections.Generic;

namespace Science.Cryptography.Ciphers;

public sealed class IgnoreCaseCharComparer : IEqualityComparer<char>
{
    public static readonly IgnoreCaseCharComparer Instance = new();

    public bool Equals(char x, char y)
    {
        if (char.IsLetter(x) && char.IsLetter(y))
            return char.ToUpperInvariant(x).Equals(char.ToUpperInvariant(y));

        return x.Equals(y);
    }

    public int GetHashCode(char obj)
    {
        if (char.IsLetter(obj)) char.ToUpperInvariant(obj).GetHashCode();
        return obj.GetHashCode();
    }
}
