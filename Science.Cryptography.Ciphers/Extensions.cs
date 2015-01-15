using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Science.Cryptography.Ciphers
{
    public static class CharExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldChar"></param>
        /// <param name="newChar"></param>
        /// <returns></returns>
        public static char ToSameCaseAs(this char newChar, char subject)
        {
            return Char.IsLower(subject) ? Char.ToLower(newChar) : newChar;
        }
    }

    public static class DictionaryExtensions
    {
        public static IReadOnlyDictionary<TValue, TKey> Swap<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source)
        {
            return source.ToDictionary(kv => kv.Value, kv => kv.Key);
        }

        public static T GetOrSame<T>(this IReadOnlyDictionary<T, T> source, T key)
        {
            return source.TryGetValue(key, out T value) ? value : key;
        }
    }
}
