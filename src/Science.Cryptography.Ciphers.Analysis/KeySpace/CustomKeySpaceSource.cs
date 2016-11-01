using System;
using System.Collections.Generic;
using System.Composition;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Reads words from a wordlist file line by line.
    /// </summary>
    [Export("Custom", typeof(IKeySpaceSource<string>))]
    public sealed class CustomKeySpaceSource<T> : IKeySpaceSource<T>
    {
        public CustomKeySpaceSource(IEnumerable<T> keys)
        {
            if (keys == null)
                throw new ArgumentNullException(nameof(keys));
            
            _keys = keys;
        }

        private readonly IEnumerable<T> _keys;

        public IEnumerable<T> GetKeys()
        {
            return _keys;
        }
    }
}
