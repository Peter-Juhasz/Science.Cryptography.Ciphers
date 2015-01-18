using System.Collections.Generic;
using System.Composition;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents Blaise de Vigenère's cipher.
    /// </summary>
    [Export("Vigenère", typeof(IKeyedCipher<>))]
    public class VigenèreCipher : IKeyedCipher<string>, ISupportsCustomCharset
    {
        public VigenèreCipher()
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

            foreach (char c in plaintext)
            {
                int idx = this.Charset.IndexOfIgnoreCase(c);

                yield return idx != -1
                    ? this.Charset[
                        (idx + this.Charset.IndexOfIgnoreCase(key[charCounter++ % key.Length])).Mod(this.Charset.Length)
                    ].ToSameCaseAs(c)
                    : c
                ;
            }

        }

        public IEnumerable<char> Decrypt(IEnumerable<char> ciphertext, string key)
        {
            int charCounter = 0;

            foreach (char c in ciphertext)
            {
                int idx = this.Charset.IndexOfIgnoreCase(c);

                yield return idx != -1
                    ? this.Charset[
                        (idx - this.Charset.IndexOfIgnoreCase(key[charCounter++ % key.Length])).Mod(this.Charset.Length)
                    ].ToSameCaseAs(c)
                    : c
                ;
            }
        }
    }
}
