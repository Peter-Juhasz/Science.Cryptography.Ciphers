using System.Collections.Generic;
using System.Threading;

namespace Science.Cryptography.Ciphers.Analysis;

public interface IAsyncKeySpace<TKey>
{
	IAsyncEnumerable<TKey> GetKeysAsync(CancellationToken cancellationToken);
}