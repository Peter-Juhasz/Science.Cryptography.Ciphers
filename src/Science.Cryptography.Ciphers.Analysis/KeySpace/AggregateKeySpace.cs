using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis.KeySpace;

/// <summary>
/// Aggregates the content of multiple <see cref="IKeySpace{TKey}"/>s.
/// </summary>
/// <typeparam name="T">Type of the key.</typeparam>
public sealed class AggregateKeySpace<T> : IKeySpace<T>
{
	public AggregateKeySpace(IReadOnlyCollection<IKeySpace<T>> sources)
	{
		Sources = sources;
	}

	private IReadOnlyCollection<IKeySpace<T>> Sources { get; }

	public IEnumerable<T> GetKeys() => Sources.SelectMany(s => s.GetKeys());
}
