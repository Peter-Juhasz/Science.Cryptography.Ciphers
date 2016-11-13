namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Contains well-known cipher keys for <see cref="ShiftCipher"/>.
    /// </summary>
    public static partial class WellKnownShiftCipherKeys
    {
        /// <summary>
        /// Julius Caesar's key for the shift cipher.
        /// </summary>
        public const int Caesar = 3;

        /// <summary>
        /// Key for the ROT-13 chiper.
        /// </summary>
        public const int Rot13 = 13;
    }
}
