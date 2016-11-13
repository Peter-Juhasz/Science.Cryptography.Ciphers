using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents Sir Francis Beaufort's cipher.
    /// </summary>
    [Export("Beaufort", typeof(IKeyedCipher<>))]
    public class BeaufortCipher : ReciprocalKeyedCipher<string>, ISupportsCustomCharset
    {
        public BeaufortCipher(string charset = Charsets.English)
        {
            if (charset == null)
                throw new ArgumentNullException(nameof(charset));

            this.Charset = charset;
            _tabulaRecta = new TabulaRecta(this.Charset);
        }

        private readonly TabulaRecta _tabulaRecta;

        public string Charset { get; private set; }


        protected override string Crypt(string text, string key)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            char[] result = new char[text.Length];
            int charCounter = 0;

            for (int i = 0; i < text.Length; i++)
            {
                int idx = this.Charset.IndexOfIgnoreCase(text[i]);

                result[i] = idx != -1
                    ? _tabulaRecta.FindColumnOrRowLabel(text[i], key[charCounter++ % key.Length]).ToSameCaseAs(text[i])
                    : text[i]
                ;
            }

            return new String(result);
        }
    }
}
