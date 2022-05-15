using Science.Cryptography.Ciphers.Specialized;
using System.Collections.Generic;
using System.Composition;

namespace Science.Cryptography.Ciphers.Analysis;

[Export("NthCharacter", typeof(IKeySpace<NthCharacterKey>))]
public sealed class NthCharacterKeySpace : IKeySpace<NthCharacterKey>
{
	public NthCharacterKeySpace(int maxOffset, int maxN)
	{
		MaxOffset = maxOffset;
		MaxN = maxN;
	}

	public int MaxOffset { get; }
	public int MaxN { get; }

	public IEnumerable<NthCharacterKey> GetKeys()
	{
		for (int o = 0; o <= MaxOffset; o++)
		for (int n = 2; n <= MaxN; n++)
			yield return new NthCharacterKey(o, n);
	}
}
