using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;

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
            if (words == null)
                throw new ArgumentNullException(nameof(words));

            _words = words;
        }
        public WordlistKeySpaceSource(string path)
            : this(File.ReadAllLines(path))
        { }

        private readonly IReadOnlyList<string> _words;


        public IEnumerable<string> GetKeys()
        {
            return _words;
        }
    }
}
