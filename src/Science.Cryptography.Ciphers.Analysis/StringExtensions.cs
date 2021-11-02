using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace Science.Cryptography.Ciphers.Analysis
{
    internal static class StringExtensions
    {
        public static IEnumerable<int> AllIndexesOf(this string str, string subject, int startIndex = 0, StringComparison comparison = StringComparison.Ordinal)
        {
            int idx = str.IndexOf(subject, startIndex, comparison);

            while (idx != -1)
            {
                yield return idx;
                idx = str.IndexOf(subject, idx + 1, comparison);
            }
        }

        public static int GetCardinality(this BitArray bitArray)
        {
            var length = (bitArray.Count >> 5) + 1;
            uint[] ints = ArrayPool<uint>.Shared.Rent(length);
            bitArray.CopyTo(ints, 0);
            int count = 0;
            for (int i = 0; i < ints.Length; i++)
            {
                count += BitOperations.PopCount(ints[i]);
            }
            ArrayPool<uint>.Shared.Return(ints);
            return count;
        }
    }
}
