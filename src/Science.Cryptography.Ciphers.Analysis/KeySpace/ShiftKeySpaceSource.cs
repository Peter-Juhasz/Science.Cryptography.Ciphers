using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis
{
    [Export("Shift", typeof(IKeySpaceSource<int>))]
    public sealed class ShiftKeySpaceSource : IKeySpaceSource<int>
    {
        public ShiftKeySpaceSource(Alphabet alphabet)
        {
            Alphabet = alphabet;
        }

        public Alphabet Alphabet { get; }

        public IEnumerable<int> GetKeys() => Enumerable.Range(1, Alphabet.Length - 1);
    }
}
