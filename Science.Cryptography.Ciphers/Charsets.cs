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
        public static readonly string English = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static readonly string EnglishWithoutJ = "ABCDEFGHIKLMNOPQRSTUVWXYZ";

        public static readonly string EnglishWithoutK = "ABCDEFGHIJLMNOPQRSTUVWXYZ";

        public static readonly string EnglishWithoutL = "ABCDEFGHIJKMNOPQRSTUVWXYZ";

        public static readonly string EnglishWithoutQ = "ABCDEFGHIJKLMNOPRSTUVWXYZ";

        /// <summary>
        /// Digits from 0, 1 to 10.
        /// </summary>
        public static string Digits
        {
            get { return "0123456789"; }
        }
    }
}
