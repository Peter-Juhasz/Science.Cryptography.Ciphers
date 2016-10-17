using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Science.Cryptography.Ciphers
{
    public static class Int32Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Mod(this int a, int b)
        {
            return a >= 0 ? a % b : (b + a) % b;
        }
    }

    public static class CharExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToUpper(this char @char)
        {
            return Char.ToUpper(@char);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToLower(this char @char)
        {
            return Char.ToLower(@char);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newChar"></param>
        /// <returns></returns>
        public static char ToSameCaseAs(this char newChar, char reference)
        {
            return Char.IsLower(reference) ? newChar.ToLower() : newChar;
        }
    }

    public static class StringExtensions
    {
        public static int IndexOfIgnoreCase(this string source, char subject)
        {
            Char toCompare = subject.ToUpper();

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].ToUpper() == toCompare)
                    return i;
            }

            return -1;
        }

        internal static IEnumerable<string> Split(this string source, int chunkSize)
        {
            int offset = 0;

            while (offset <= source.Length - chunkSize)
            {
                yield return source.Substring(offset, chunkSize);

                offset += chunkSize;
            }
        }
    }

    internal static class ArrayExtensions
    {
        public static T[,] RotateClockwise<T>(this T[,] source)
        {
            int width = source.GetLength(0), height = source.GetLength(1);

            T[,] result = new T[height, width];

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                result[height - y - 1, x] = source[x, y];

            return result;
        }

        public static T[,] RotateCounterClockwise<T>(this T[,] source)
        {
            int width = source.GetLength(0), height = source.GetLength(1);

            T[,] result = new T[height, width];

            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                result[y, width - x - 1] = source[x, y];

            return result;
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
            T value;
            return source.TryGetValue(key, out value) ? value : key;
        }
    }
}
