using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Kama-Sutra cipher.
    /// </summary>
    [Export("Kamasutra", typeof(IKeyedCipher<>))]
    public class KamaSutraCipher : ReciprocalKeyedCipher<IReadOnlyDictionary<char, char>>
    {
        protected override string Crypt(string text, IReadOnlyDictionary<char, char> key)
        {
            IReadOnlyDictionary<char, char> precachedKey = PreCacheReverseKey(key);

            return text.EfficientSelect(c => precachedKey.GetOrSame(c).ToSameCaseAs(c));
        }

        private static IReadOnlyDictionary<char, char> PreCacheReverseKey(IReadOnlyDictionary<char, char> key)
        {
            Dictionary<char, char> merged = new Dictionary<char, char>();
            key.ForEach(kv => merged.Add(kv.Key, kv.Value));
            key.Swap().ForEach(kv => merged.Add(kv.Key, kv.Value));
            return merged;
        }
    }
}
