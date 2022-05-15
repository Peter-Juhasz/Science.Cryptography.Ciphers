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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsUpperAsciiLetter(this char newChar) => newChar >= 'A' && newChar <= 'Z';

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLowerAsciiLetter(this char newChar) => newChar >= 'a' && newChar <= 'z';

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAsciiLetter(this char newChar) => newChar.IsUpperAsciiLetter() || newChar.IsLowerAsciiLetter();
}
