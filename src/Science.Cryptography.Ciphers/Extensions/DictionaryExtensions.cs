using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    public static partial class DictionaryExtensions
    {
        public static IReadOnlyDictionary<TValue, TKey> Swap<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source) => source.ToDictionary(kv => kv.Value, kv => kv.Key);

        public static IReadOnlyDictionary<TValue, TKey> SwapIgnoreDuplicates<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source)
        {
            var result = new Dictionary<TValue, TKey>();
            foreach (var kvp in source)
            {
                result[kvp.Value] = kvp.Key;
            }
            return result;
        }

        public static T GetOrSame<T>(this IReadOnlyDictionary<T, T> source, T key) => source.TryGetValue(key, out T value) ? value : key;
    }
}
