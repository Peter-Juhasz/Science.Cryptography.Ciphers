using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    public static partial class DictionaryExtensions
    {
        public static IReadOnlyDictionary<TValue, TKey> Swap<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source)
        {
            return source.ToDictionary(kv => kv.Value, kv => kv.Key);
        }

        public static T GetOrSame<T>(this IReadOnlyDictionary<T, T> source, T key)
        {
            T value;
            return source.TryGetValue(key, out value) ? value : key;
        }
    }
}
