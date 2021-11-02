using System.Runtime.CompilerServices;

namespace Science.Cryptography.Ciphers;

public static partial class Int32Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Mod(this int a, int b)
    {
        return a >= 0 ? a % b : (b + a) % b;
    }
}
