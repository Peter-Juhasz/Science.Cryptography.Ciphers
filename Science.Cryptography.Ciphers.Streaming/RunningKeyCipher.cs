using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents the Running Key cipher.
    /// </summary>
    public class RunningKeyCipher : IKeyedCipher<string>, ISupportsCustomCharset
    {
        public RunningKeyCipher()
        {
            this.Charset = Charsets.English;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }

        public IEnumerable<char> Encrypt(IEnumerable<char> plaintext, string key)
        {
            int charCounter = 0;

            key = key.ToUpper();

            foreach (char c in plaintext)
            {
                int idx = this.Charset.IndexOfIgnoreCase(c);

                yield return idx != -1
                    ? (this.Charset[(idx + this.Charset.IndexOf(key[charCounter++])) % this.Charset.Length]).ToSameCaseAs(c)
                    : c
                ;
            }
        }

        public IEnumerable<char> Decrypt(IEnumerable<char> ciphertext, string key)
        {
            int charCounter = 0;

            key = key.ToUpper();

            foreach (char c in ciphertext)
            {
                int idx = this.Charset.IndexOfIgnoreCase(c);

                yield return idx != -1
                    ? (this.Charset[(idx - this.Charset.IndexOf(key[charCounter++])).Mod(this.Charset.Length)]).ToSameCaseAs(c)
                    : c
                ;
            }
        }
    }
}
