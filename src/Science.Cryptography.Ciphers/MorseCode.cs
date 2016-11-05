using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Morse Code cipher.
    /// </summary>
    [Export("Morse Code", typeof(ICipher))]
    public class MorseCode : ICipher
    {
        public MorseCode(MorseCodeConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _configuration = configuration;

            _dictionary = new Dictionary<char, string>()
            {
                { 'A', $"{configuration.Dot}{configuration.Dash}" },
                { 'B', $"{configuration.Dash}{configuration.Dot}{configuration.Dot}{configuration.Dot}" },
                { 'C', $"{configuration.Dash}{configuration.Dot}{configuration.Dash}{configuration.Dot}" },
                { 'D', $"{configuration.Dash}{configuration.Dot}{configuration.Dot}" },
                { 'E', $"{configuration.Dot}" },
                { 'F', $"{configuration.Dot}{configuration.Dot}{configuration.Dash}{configuration.Dot}" },
                { 'G', $"{configuration.Dash}{configuration.Dash}{configuration.Dot}" },
                { 'H', $"{configuration.Dot}{configuration.Dot}{configuration.Dot}{configuration.Dot}" },
                { 'I', $"{configuration.Dot}{configuration.Dot}" },
                { 'J', $"{configuration.Dot}{configuration.Dash}{configuration.Dash}{configuration.Dash}" },
                { 'K', $"{configuration.Dash}{configuration.Dot}{configuration.Dash}" },
                { 'L', $"{configuration.Dot}{configuration.Dash}{configuration.Dot}{configuration.Dot}" },
                { 'M', $"{configuration.Dash}{configuration.Dash}" },
                { 'N', $"{configuration.Dash}{configuration.Dot}" },
                { 'O', $"{configuration.Dash}{configuration.Dash}{configuration.Dash}" },
                { 'P', $"{configuration.Dot}{configuration.Dash}{configuration.Dash}{configuration.Dot}" },
                { 'Q', $"{configuration.Dash}{configuration.Dash}{configuration.Dot}{configuration.Dash}" },
                { 'R', $"{configuration.Dot}{configuration.Dash}{configuration.Dot}" },
                { 'S', $"{configuration.Dot}{configuration.Dot}{configuration.Dot}" },
                { 'T', $"{configuration.Dash}" },
                { 'U', $"{configuration.Dot}{configuration.Dot}{configuration.Dash}" },
                { 'V', $"{configuration.Dot}{configuration.Dot}{configuration.Dot}{configuration.Dash}" },
                { 'W', $"{configuration.Dot}{configuration.Dash}{configuration.Dash}" },
                { 'X', $"{configuration.Dash}{configuration.Dot}{configuration.Dot}{configuration.Dash}" },
                { 'Y', $"{configuration.Dash}{configuration.Dot}{configuration.Dash}{configuration.Dash}" },
                { 'Z', $"{configuration.Dash}{configuration.Dash}{configuration.Dot}{configuration.Dot}" },
            };
        }
        public MorseCode()
            : this(MorseCodeConfiguration.Default)
        { }

        private readonly MorseCodeConfiguration _configuration;
        private readonly IReadOnlyDictionary<char, string> _dictionary;


        public string Encrypt(string plaintext)
        {
            return String.Join(_configuration.Separator.ToString(),
                plaintext
                .Select(
                    ch => this._dictionary.ContainsKey(Char.ToUpper(ch)) ? this._dictionary[Char.ToUpper(ch)] : ch.ToString()
                )
            );
        }

        public string Decrypt(string ciphertext)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder window = new StringBuilder();

            IReadOnlyDictionary<string, char> decryptionDictionary = _dictionary.Swap();

            bool nextIsSeparator = true;

            foreach (char ch in ciphertext)
            {
                if (ch == _configuration.Dash || ch == _configuration.Dot)
                {
                    window.Append(ch);
                    nextIsSeparator = true;
                }
                else
                {
                    if (this._dictionary.Values.Contains(window.ToString()))
                    {
                        result.Append(decryptionDictionary[window.ToString()]);
                        window.Remove(0, window.Length);
                    }

                    window.Remove(0, window.Length);

                    if (ch != _configuration.Separator || (!nextIsSeparator && ch == _configuration.Separator))
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
    }
}
