using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers;

public class CharacterSubstitutionMap
{
	public CharacterSubstitutionMap(IReadOnlyDictionary<char, char> encryptionMap)
	{
		EncryptionMap = encryptionMap.ToDictionary(k => k.Key, k => k.Value, IgnoreCaseCharComparer.Instance);
		DecryptionMap = encryptionMap.Swap();
	}

	public IReadOnlyDictionary<char, char> EncryptionMap { get; }
	public IReadOnlyDictionary<char, char> DecryptionMap { get; }

	public char LookupOrSame(char ch)
	{
		if (EncryptionMap.TryGetValue(ch, out var translated))
		{
			return translated.ToSameCaseAs(ch);
		}

		return ch;
	}

	public char ReverseLookupOrSame(char ch)
	{
		if (DecryptionMap.TryGetValue(ch, out var translated))
		{
			return translated.ToSameCaseAs(ch);
		}

		return ch;
	}
}
