using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Represents partitioned key space, which is optimized for parallel processing where each partition can preserve a state to avoid allocations.
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IPartitionedKeySpace<TKey>
{
	IEnumerable<IKeySpace<TKey>> GetPartitions(int? desiredCount = null);
}