using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis.KeySpace
{
    /// <summary>
    /// Aggregates the content of multiple <see cref="IKeySpaceSource{TKey}"/>s.
    /// </summary>
    /// <typeparam name="T">Type of the key.</typeparam>
    public sealed class AggregateKeySpace<T> : IKeySpaceSource<T>
    {
        public AggregateKeySpace(IReadOnlyCollection<IKeySpaceSource<T>> sources)
        {
            if (sources == null)
                throw new ArgumentNullException(nameof(sources));

            _sources = sources;
        }

        private readonly IReadOnlyCollection<IKeySpaceSource<T>> _sources;

        public IEnumerable<T> GetKeys() =>
            from source in _sources
            from key in source.GetKeys()
            select key
        ;
    }
}
