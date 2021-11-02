using System.Collections.Generic;
using System.Composition;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Reads words from a wordlist file line by line.
    /// </summary>
    [Export("Wordlist", typeof(IKeySpaceSource<string>))]
    public sealed class WordlistKeySpaceSource : IKeySpaceSource<string>
    {
        public WordlistKeySpaceSource(IReadOnlyList<string> words)
        {
            _words = words;
        }

        private readonly IReadOnlyList<string> _words;


        public IEnumerable<string> GetKeys() => _words;
    }
}
