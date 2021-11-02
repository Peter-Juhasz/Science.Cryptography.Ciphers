using System;
using System.Runtime.CompilerServices;

namespace Science.Cryptography.Ciphers;

public static partial class CharExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLetter(this char @char) => Char.IsLetter(@char);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUpper(this char @char) => Char.IsUpper(@char);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char ToUpper(this char @char) => Char.ToUpperInvariant(@char);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char ToLower(this char @char) => Char.ToLowerInvariant(@char);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char ToSameCaseAs(this char newChar, char reference) => Char.IsLower(reference) ? newChar.ToLower() : newChar;
}
