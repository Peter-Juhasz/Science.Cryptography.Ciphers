using System;

namespace Science.Cryptography.Ciphers
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


		public string Encrypt(string plaintext, string key)
		{
			char[] result = new char[plaintext.Length];
			int charCounter = 0;

			for (int i = 0; i < plaintext.Length; i++)
			{
				int idx = this.Charset.IndexOf(plaintext[i].ToString(), StringComparison.OrdinalIgnoreCase);

				if (idx != -1)
                    result[i] = (this.Charset[(idx + this.Charset.IndexOf(key[charCounter++ % key.Length].ToString(), StringComparison.CurrentCultureIgnoreCase)).Mod(this.Charset.Length)]).ToSameCaseAs(plaintext[i]);
				else
					result[i] = plaintext[i];
			}

			return new String(result);
		}

		public string Decrypt(string ciphertext, string key)
		{
			char[] result = new char[ciphertext.Length];
			int charCounter = 0;

            for (int i = 0; i < ciphertext.Length; i++)
			{
				int idx = this.Charset.IndexOf(ciphertext[i].ToString(), StringComparison.OrdinalIgnoreCase);

				if (idx != -1)
					result[i] = (this.Charset[(idx - this.Charset.IndexOf(key[charCounter++ % key.Length].ToString(), StringComparison.CurrentCultureIgnoreCase)).Mod(this.Charset.Length)]).ToSameCaseAs(ciphertext[i]);
				else
					result[i] = ciphertext[i];
			}

			return new String(result);
		}
	}
}
