using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents the Monoalphabetic Substitution cipher.
    /// </summary>
    [Export("Monoalphabetic Substitution", typeof(IKeyedCipher<>))]
    public sealed class MonoalphabeticSubstitutionCipher : IKeyedCipher<IReadOnlyDictionary<char, char>>
    {
        public IEnumerable<char> Encrypt(IEnumerable<char> input, IReadOnlyDictionary<char, char> key)
        {
            return input.Select(s => key.GetOrSame(s));
        }

        public IEnumerable<char> Decrypt(IEnumerable<char> input, IReadOnlyDictionary<char, char> key)
        {
            var decryptionKey = key.Swap();

            return input.Select(s => decryptionKey.GetOrSame(s));
        }
    }
}
