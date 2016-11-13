using System;
using System.Composition;
using System.Linq;
using System.Text.RegularExpressions;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Null cipher.
    /// </summary>
    [Export("Gudhayojya", typeof(IKeyedCipher<>))]
    public class GudhayojyaCipher : IKeyedCipher<string>
    {
        private readonly static Regex WordRegex = new Regex(@"\w+");

        public string Encrypt(string plaintext, string key)
        {
            string capitalizedKey = key.First().ToUpper() + key.Substring(1);
            return new Regex(@"\w+").Replace(plaintext, m => 
                m.Value.First().IsUpper()
                    ? capitalizedKey + m.Value.ToLowerInvariant()
                    : key + m.Value
            );
        }

        public string Decrypt(string ciphertext, string key)
        {
            return new Regex($@"\b{key}", RegexOptions.IgnoreCase).Replace(ciphertext, String.Empty);
        }
    }
}
