using System;
using System.Collections.Generic;

namespace Science.Cryptography.Ciphers.Streaming
{
    /// <summary>
    /// Represents Blaise de Vigenère's cipher.
    /// </summary>
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
				int idx = this.Charset.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase);

                yield return idx != -1
                    ? (
                        this.Charset[
                            (idx + this.Charset.IndexOf(key[charCounter++ % key.Length].ToString(), StringComparison.CurrentCultureIgnoreCase)).Mod(this.Charset.Length)
                        ]
                    ).ToSameCaseAs(c)
                    : c
                ;
			}

		}

		public IEnumerable<char> Decrypt(IEnumerable<char> ciphertext, string key)
		{
			int charCounter = 0;

            foreach (char c in ciphertext)
            {
				int idx = this.Charset.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase);

                yield return idx != -1
                    ? (
                        this.Charset[
                            (idx - this.Charset.IndexOf(key[charCounter++ % key.Length].ToString(), StringComparison.CurrentCultureIgnoreCase)).Mod(this.Charset.Length)
                        ]
                    ).ToSameCaseAs(c)
                    : c
                ;
			}
		}
	}
}
