using System;

namespace Science.Cryptography.Ciphers.Analysis
{
    public class KeyFinderResult
    {
        public KeyFinderResult(string speculativePlaintext, double rank)
        {
            if (speculativePlaintext == null)
                throw new ArgumentNullException(nameof(speculativePlaintext));

            this.SpeculativePlaintext = speculativePlaintext;
            this.Rank = rank;
        }

        public double Rank { get; set; }

        public string SpeculativePlaintext { get; set; }
    }
}
