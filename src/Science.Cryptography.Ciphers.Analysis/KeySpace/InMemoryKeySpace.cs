using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

public record class InMemoryKeySpace<T>(IEnumerable<T> Keys) : IKeySpace<T>
{
	public IEnumerable<T> GetKeys() => Keys;
}