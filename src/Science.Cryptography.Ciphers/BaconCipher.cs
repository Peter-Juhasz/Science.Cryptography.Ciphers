using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents Francis Bacon's cipher.
    /// </summary>
    [Export("Bacon", typeof(ICipher))]
    public class BaconCipher : ICipher
    {
        public BaconCipher()
        {
            this.Dictionary = new Dictionary<char, string>()
            {
                { 'A', "AAAAA" },
                { 'B', "AAAAB" },
                { 'C', "AAABA" },
                { 'D', "AAABB" },
                { 'E', "AABAA" },
                { 'F', "AABAB" },
                { 'G', "AABBA" },
                { 'H', "AABBB" },
                { 'I', "ABAAA" },
                { 'J', "ABAAA" },
                { 'K', "ABAAB" },
                { 'L', "ABABA" },
                { 'M', "ABABB" },
                { 'N', "ABBAA" },
                { 'O', "ABBAB" },
                { 'P', "ABBBA" },
                { 'Q', "ABBBB" },
                { 'R', "BAAAA" },
                { 'S', "BAAAB" },
                { 'T', "BAABA" },
                { 'U', "BAABB" },
                { 'V', "BAABB" },
                { 'W', "BABAA" },
                { 'X', "BABAB" },
                { 'Y', "BABBA" },
                { 'Z', "BABBB" },
            };
        }

        private readonly IReadOnlyDictionary<char, string> Dictionary;


        public string Encrypt(string plaintext)
        {
            StringBuilder result = new StringBuilder();

            foreach (char ch in plaintext)
                result.Append(this.Dictionary.ContainsKey(Char.ToUpper(ch)) ? this.Dictionary[Char.ToUpper(ch)] : ch.ToString());

            return result.ToString();
        }

        public string Decrypt(string ciphertext)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder window = new StringBuilder(5);

            foreach (char ch in ciphertext)
            {
                if (ch == 'A' || ch == 'B')
                {
                    window.Append(ch);

                    if (window.Length == 5)
                    {
                        if (this.Dictionary.Values.Contains(window.ToString()))
                            result.Append(this.Dictionary.First(kvp => kvp.Value.Equals(window.ToString(), StringComparison.OrdinalIgnoreCase)).Key);
                        else
                            result.Append(window.ToString());

                        window.Clear();
                    }
                }
                else
                {
                    result.Append(ch);
                    window.Clear();
                }
            }

            if (window.Length == 5)
                result.Append(this.Dictionary.First(kvp => kvp.Value.Equals(window.ToString(), StringComparison.OrdinalIgnoreCase)).Key);

            return result.ToString();
        }
    }
}
