using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;

namespace Science.Cryptography.Ciphers
{
    using Analysis;

    /// <summary>
    /// Represents the Morse Code cipher.
    [Export("Morse Code", typeof(ICipher))]
    /// </summary>
    public class MorseCode : ICipher, ISupportsRecognition
    {
        public MorseCode()
        {
            this.Dictionary = new Dictionary<char, string>()
            {
                { 'A', ".-" },
                { 'B', "-..." },
                { 'C', "-.-." },
                { 'D', "-.." },
                { 'E', "." },
                { 'F', "..-." },
                { 'G', "--." },
                { 'H', "...." },
                { 'I', ".." },
                { 'J', ".---" },
                { 'K', "-.-" },
                { 'L', ".-.." },
                { 'M', "--" },
                { 'N', "-." },
                { 'O', "---" },
                { 'P', ".--." },
                { 'Q', "--.-" },
                { 'R', ".-." },
                { 'S', "..." },
                { 'T', "-" },
                { 'U', "..-" },
                { 'V', "...-" },
                { 'W', ".--" },
                { 'X', "-..-" },
                { 'Y', "-.--" },
                { 'Z', "--.." },
            };
        }

        private readonly IReadOnlyDictionary<char, string> Dictionary;


        public string Encrypt(string plaintext)
        {
            return String.Join(" ",
                plaintext
                .Select(
                    ch => this.Dictionary.ContainsKey(Char.ToUpper(ch)) ? this.Dictionary[Char.ToUpper(ch)] : ch.ToString()
                )
            );
        }

        public string Decrypt(string ciphertext)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder window = new StringBuilder();

            IReadOnlyDictionary<string, char> decryptionDictionary = Dictionary.Swap();

            bool nextIsSeparator = true;

            foreach (char ch in ciphertext)
            {
                if (ch == '-' || ch == '.')
                {
                    window.Append(ch);
                    nextIsSeparator = true;
                }
                else
                {
                    if (this.Dictionary.Values.Contains(window.ToString()))
                    {
                        result.Append(decryptionDictionary[window.ToString()]);
                        window.Remove(0, window.Length);
                    }

                    window.Remove(0, window.Length);

                    if (ch != ' ' || (!nextIsSeparator && ch == ' '))
                    {
                        result.Append(ch);
                        nextIsSeparator = true;
                    }
                    else if (ch == ' ')
                        nextIsSeparator = false;
                }
            }

            if (window.Length > 0)
                result.Append(decryptionDictionary[window.ToString()]);

            return result.ToString();
        }


        public bool Recognize(string ciphertext)
        {
            return ciphertext
                .Where(c => !Char.IsWhiteSpace(c))
                .MostOfAll(c => c == '.' || c == '-')
            ;
        }
    }
}
