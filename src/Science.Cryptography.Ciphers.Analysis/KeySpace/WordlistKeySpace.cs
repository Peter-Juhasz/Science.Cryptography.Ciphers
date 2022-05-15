using System.Collections.Generic;
using System.Composition;

namespace Science.Cryptography.Ciphers.Analysis;

[Export("Wordlist", typeof(IKeySpace<string>))]
public sealed record class InMemoryWordlistKeySpace : InMemoryKeySpace<string>
{
	public InMemoryWordlistKeySpace(IReadOnlyList<string> words)
		: base(words)
	{ }
}
