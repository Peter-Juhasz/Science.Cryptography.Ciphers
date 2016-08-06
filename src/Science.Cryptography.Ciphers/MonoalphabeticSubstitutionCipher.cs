using System;
using System.Collections.Generic;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Monoalphabetic Substitution cipher.
    /// </summary>
    [Export("Monoalphabetic Substitution", typeof(IKeyedCipher<>))]
    public class MonoalphabeticSubstitutionCipher : IKeyedCipher<IReadOnlyDictionary<char, char>>
    {
        public string Encrypt(string plaintext, IReadOnlyDictionary<char, char> key)
        {
            char[] ciphertext = new char[plaintext.Length];

            for (int i = 0; i < plaintext.Length; i++)
            {
                ciphertext[i] = key.GetOrSame(plaintext[i]).ToSameCaseAs(plaintext[i]);
            }

            return new String(ciphertext);
        }

        public string Decrypt(string ciphertext, IReadOnlyDictionary<char, char> key)
        {
            char[] plaintext = new char[ciphertext.Length];

            var decryptionKey = key.Swap();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                plaintext[i] = decryptionKey.GetOrSame(ciphertext[i]).ToSameCaseAs(ciphertext[i]);
            }

            return new String(plaintext);
        }
    }
}
