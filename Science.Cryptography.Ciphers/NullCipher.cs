using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Null cipher.
    /// </summary>
    public class NullCipher : IKeyedCipher<IReadOnlyList<int>>
    {
        private readonly static Regex WordRegex = new Regex(@"\w+");

        public string Encrypt(string plaintext, IReadOnlyList<int> key)
        {
            StringBuilder result = new StringBuilder();
            int keyIndex = 0;

            foreach (string word in this.GetWords(plaintext))
            {
                result.Append(word[key[keyIndex]]);

                // advance key
                keyIndex++;

                if (keyIndex == key.Count)
                    keyIndex = 0;
            }

            return result.ToString();
        }

        public string Decrypt(string ciphertext, IReadOnlyList<int> key)
        {
            throw new NotSupportedException();
        }


        protected IEnumerable<string> GetWords(string text)
        {
            return WordRegex.Matches(text).Cast<Match>().Select(m => m.Value);
        }
    }
}
