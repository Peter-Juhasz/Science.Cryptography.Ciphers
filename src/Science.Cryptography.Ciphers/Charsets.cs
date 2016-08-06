namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Contains a set of alphabets.
    /// </summary>
    public static class Charsets
    {
        /// <summary>
        /// The regular english alphabet.
        /// </summary>
        public const string English = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public const string EnglishWithoutJ = "ABCDEFGHIKLMNOPQRSTUVWXYZ";

        public const string EnglishWithoutK = "ABCDEFGHIJLMNOPQRSTUVWXYZ";

        public const string EnglishWithoutL = "ABCDEFGHIJKMNOPQRSTUVWXYZ";

        public const string EnglishWithoutQ = "ABCDEFGHIJKLMNOPRSTUVWXYZ";

        /// <summary>
        /// Digits from 0, 1 to 10.
        /// </summary>
        public const string Digits = "0123456789";
    }
}
