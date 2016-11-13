using System;
using System.Runtime.CompilerServices;

namespace Science.Cryptography.Ciphers
{
    public static partial class CharExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUpper(this char @char)
        {
            return Char.IsUpper(@char);
        }

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
}
