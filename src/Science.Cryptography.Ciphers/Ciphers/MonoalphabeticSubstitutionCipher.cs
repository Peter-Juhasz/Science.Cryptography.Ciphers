using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Represents the Monoalphabetic Substitution cipher.
/// </summary>
[Export("Monoalphabetic Substitution", typeof(IKeyedCipher<>))]
public class MonoalphabeticSubstitutionCipher : IKeyedCipher<CharacterSubstitutionMap>
{
	public void Encrypt(ReadOnlySpan<char> text, Span<char> result, CharacterSubstitutionMap key, out int written)
	{
		for (int i = 0; i < text.Length; i++)
		{
			var ch = text[i];
			result[i] = key.LookupOrSame(ch);
		}

		written = text.Length;
	}

	public void Decrypt(ReadOnlySpan<char> text, Span<char> result, CharacterSubstitutionMap key, out int written)
	{
		for (int i = 0; i < text.Length; i++)
		{
			var ch = text[i];
			result[i] = key.ReverseLookupOrSame(ch);
		}

		written = text.Length;
	}
}
