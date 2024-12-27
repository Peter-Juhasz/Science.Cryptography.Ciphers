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
	public static char ToUpperInvariant(this char @char) => Char.ToUpperInvariant(@char);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static char ToLowerInvariant(this char @char) => Char.ToLowerInvariant(@char);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static char ToSameCaseAs(this char newChar, char reference) => Char.IsLower(reference) ? newChar.ToLowerInvariant() : newChar;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsUpperAsciiLetter(this char newChar) => Char.IsAsciiLetterUpper(newChar);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsLowerAsciiLetter(this char newChar) => Char.IsAsciiLetterLower(newChar);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAsciiLetter(this char newChar) => Char.IsAsciiLetter(newChar);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsAsciiDigit(this char newChar) => Char.IsAsciiDigit(newChar);
}
