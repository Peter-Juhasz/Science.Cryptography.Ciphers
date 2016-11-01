using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis
{
    [Export("Affine", typeof(IKeySpaceSource<AffineKey>))]
    public sealed class AffineKeySpaceSource : IKeySpaceSource<AffineKey>
    {
        public AffineKeySpaceSource(string charset = Charsets.English)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            this.Charset = charset;
        }

        public string Charset { get; private set; }

        public IEnumerable<AffineKey> GetKeys()
        {
            int n = this.Charset.Length;

            return
                from a in CoprimesTo(n)
                from b in Enumerable.Range(0, n)
                select new AffineKey(a, b)
            ;
        }

        private static int Gcd(int a, int b)
        {
            int rem;

            while (b != 0)
            {
                rem = a % b;
                a = b;
                b = rem;
            }

            return a;
        }

        private static IEnumerable<int> CoprimesTo(int n)
        {
            return Enumerable.Range(0, n)
                .Where(i => Gcd(i, n) == 1)
            ;
        }
    }
}
