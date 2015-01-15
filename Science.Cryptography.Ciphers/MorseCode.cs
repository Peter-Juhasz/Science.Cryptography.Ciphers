using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Morse Code cipher.
    /// </summary>
    public class MorseCode : ICipher
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
                { 'Z', "--." }
            };
        }

        private readonly IDictionary<char, string> Dictionary;


        public string Encrypt(string plaintext)
        {
            return String.Join(" ",
                plaintext.ToCharArray()
                .Select(
                    ch => this.Dictionary.ContainsKey(Char.ToUpper(ch)) ? this.Dictionary[Char.ToUpper(ch)] : ch.ToString()
                )
            );
        }

        public string Decrypt(string ciphertext)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder window = new StringBuilder();

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
                        result.Append(this.Dictionary.First(kvp => kvp.Value.CompareTo(window.ToString()) == 0).Key);
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
                result.Append(this.Dictionary.First(kvp => kvp.Value.CompareTo(window.ToString()) == 0).Key);

            return result.ToString();
        }
    }
}
