using System;
using System.Collections.Generic;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Monoalphabetic Substitution cipher.
    /// </summary>
    public class MonoalphabeticSubstitutionCipher : IKeyedCipher<IReadOnlyDictionary<char, char>>
    {
        public string Encrypt(string plaintext, IReadOnlyDictionary<char, char> key)
        {
            char[] ciphertext = new char[plaintext.Length];

            for (int i = 0; i < plaintext.Length; i++)
            {
                if (key.ContainsKey(Char.ToUpper(plaintext[i])))
                    ciphertext[i] = key[plaintext[i]].ToSameCaseAs(plaintext[i]);
                else
                    ciphertext[i] = plaintext[i];
            }

            return new String(ciphertext);
        }

        public string Decrypt(string ciphertext, IReadOnlyDictionary<char, char> key)
        {
            char[] plaintext = new char[ciphertext.Length];

            for (int i = 0; i < ciphertext.Length; i++)
            {
                if (key.Values.Contains(Char.ToUpper(ciphertext[i])))
                    plaintext[i] = key[key.First(kv => kv.Value == Char.ToUpper(ciphertext[i])).Key].ToSameCaseAs(ciphertext[i]);
                else
                    plaintext[i] = ciphertext[i];
            }

            return new String(plaintext);
        }
    }
}
