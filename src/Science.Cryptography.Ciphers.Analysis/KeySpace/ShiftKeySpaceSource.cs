using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis
{
    [Export("Shift", typeof(IKeySpaceSource<int>))]
    public sealed class ShiftKeySpaceSource : IKeySpaceSource<int>
    {
        public ShiftKeySpaceSource(string charset = Charsets.English)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            this.Charset = charset;
        }

        public string Charset { get; private set; }

        public IEnumerable<int> GetKeys()
        {
            return Enumerable.Range(1, this.Charset.Length - 1);
        }
    }
}
