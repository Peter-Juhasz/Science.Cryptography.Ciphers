using System;

namespace Science.Cryptography.Ciphers.Analysis
{
    public class KeyFinderResult<TKey>
    {
        public KeyFinderResult(TKey key, string speculativePlaintext, double rank)
        {
            if (speculativePlaintext == null)
                throw new ArgumentNullException(nameof(speculativePlaintext));

            this.Key = key;
            this.SpeculativePlaintext = speculativePlaintext;
            this.Rank = rank;
        }

        public double Rank { get; private set; }

        public TKey Key { get; private set; }

        public string SpeculativePlaintext { get; private set; }
    }
}
