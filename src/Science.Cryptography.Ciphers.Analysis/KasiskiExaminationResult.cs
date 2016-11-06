using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Analysis
{
    public class KasiskiExaminationResult
    {
        public KasiskiExaminationResult(IReadOnlyCollection<int> speculativeKeyLength)
        {
            this.SpeculativeKeyLengths = speculativeKeyLength;
        }

        public IReadOnlyCollection<int> SpeculativeKeyLengths { get; private set; }
    }
}
