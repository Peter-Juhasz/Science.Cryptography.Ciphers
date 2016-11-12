using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Atbash cipher.
    /// </summary>
    [Export("Atbash", typeof(ICipher))]
    public class AtbashCipher : ReciprocalCipher, ISupportsCustomCharset
    {
        public AtbashCipher(string charset = Charsets.English)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            this.Charset = charset;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Charset { get; set; }

        protected override string Crypt(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            char[] result = new char[text.Length];

            for (int i = 0; i < text.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(text[i]);

                result[i] = idx != -1
                    ? this.Charset[this.Charset.Length - idx - 1].ToSameCaseAs(text[i])
                    : text[i]
                ;
            }

            return new String(result);
        }
    }
}
