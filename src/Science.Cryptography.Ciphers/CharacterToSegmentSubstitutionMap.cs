using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers;

public class CharacterToSegmentSubstitutionMap
{
	public CharacterToSegmentSubstitutionMap(IReadOnlyDictionary<char, string> encryptionMap)
	{
		EncryptionMap = encryptionMap;
		DecryptionMap = encryptionMap.SwapIgnoreDuplicates(StringComparer.OrdinalIgnoreCase);
	}

	public IReadOnlyDictionary<char, string> EncryptionMap { get; }
	public IReadOnlyDictionary<string, char> DecryptionMap { get; }

	public bool TryLookup(char ch, out string result) => EncryptionMap.TryGetValue(ch, out result);

	public bool TryLookup(char ch, Span<char> result, ref int written)
	{
		if (EncryptionMap.TryGetValue(ch, out var translated))
		{
			translated.CopyTo(result);
			written += translated.Length;
			return true;
		}

		return false;
	}

	public bool TryLookup(char ch, in SpanWriter<char> result)
	{
		if (EncryptionMap.TryGetValue(ch, out var translated))
		{
			result.Write(translated);
			return true;
		}

		return false;
	}

	public bool TryReverseLookup(ReadOnlySpan<char> segment, out char result)
	{
		if (DecryptionMap.TryGetValue(new string(segment), out var translated))
		{
			result = translated;
			return true;
		}

		result = default;
		return false;
	}
}