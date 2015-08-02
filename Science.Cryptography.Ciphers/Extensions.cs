using System;
using System.Collections;
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
        /// <param name="oldChar"></param>
        /// <param name="newChar"></param>
        /// <returns></returns>
        public static char ToSameCaseAs(this char newChar, char reference)
        {
            return Char.IsLower(reference) ? newChar.ToLower() : newChar;
        }
    }

    public static class StringExtensions
    {
        public static IEnumerable<char> AsEnumerable(this string source)
        {
            return new CharEnumerable(source);
        }

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

        public static IEnumerable<string> Split(this string source, int chunkSize)
        {
            int offset = 0;

            while (offset <= source.Length - chunkSize)
            {
                yield return source.Substring(offset, chunkSize);

                offset += chunkSize;
            }
        }
    }

    public static class ArrayExtensions
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

    public static class EnumerableExtensions
    {
        public static bool MostOfAll<T>(this IEnumerable<T> source, Func<T, bool> predicate, double threshold = 0.9)
        {
            int count = source.Count(),
                satisfyingCount = source.Where(predicate).Count();

            return threshold <= satisfyingCount / (double)count;
        }
    }


    #region String : IEnumerable<char>
    internal sealed class CharEnumerable : IEnumerable<char>
    {
        internal CharEnumerable(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            this.str = str;
        }

        private string str;

        public IEnumerator<char> GetEnumerator()
        {
            return new CharEnumerator(this.str);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new CharEnumerator(this.str);
        }
    }

    // http://referencesource.microsoft.com/#mscorlib/system/charenumerator.cs,0969ce79660adf73
    internal sealed class CharEnumerator : IEnumerator, IEnumerator<char>, IDisposable
    {
        internal CharEnumerator(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            this.str = str;
            this.index = -1;
        }

        private string str;
        private int index;
        private char currentElement;

        public bool MoveNext()
        {
            if (index < (str.Length - 1))
            {
                index++;
                currentElement = str[index];
                return true;
            }
            else
                index = str.Length;

            return false;

        }

        public void Dispose()
        {
            if (str != null)
                index = str.Length;

            str = null;
        }

        /// <internalonly/>
        object IEnumerator.Current
        {
            get
            {
                if (index == -1)
                    throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");

                if (index >= str.Length)
                    throw new InvalidOperationException("Enumeration already finished.");

                return currentElement;
            }
        }

        public char Current
        {
            get
            {
                if (index == -1)
                    throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");

                if (index >= str.Length)
                    throw new InvalidOperationException("Enumeration already finished.");
                return currentElement;
            }
        }

        public void Reset()
        {
            currentElement = (char)0;
            index = -1;
        }
    }
    #endregion
}
