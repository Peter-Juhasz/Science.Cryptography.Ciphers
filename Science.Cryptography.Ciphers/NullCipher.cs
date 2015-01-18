using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Null cipher.
    [Export("Null", typeof(IKeyedCipher<>))]
    /// </summary>
    public class NullCipher : IKeyedCipher<IReadOnlyList<int>>
    {
        private readonly static Regex WordRegex = new Regex(@"\w+");

        public string Encrypt(string plaintext, IReadOnlyList<int> key)
        {
            throw new NotSupportedException();
        }

        public string Decrypt(string ciphertext, IReadOnlyList<int> key)
        {
            StringBuilder result = new StringBuilder();
            int keyIndex = 0;

            foreach (string word in this.GetWords(ciphertext))
            {
                result.Append(word[key[keyIndex]]);

                // advance key
                keyIndex++;

                if (keyIndex == key.Count)
                    keyIndex = 0;
            }

            return result.ToString();
        }


        protected IEnumerable<string> GetWords(string text)
        {
            return WordRegex.Matches(text).Cast<Match>().Select(m => m.Value);
        }
    }
}
