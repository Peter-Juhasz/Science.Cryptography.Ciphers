using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents Johannes Trithemius' cipher.
    /// </summary>
    [Export("Trithemius", typeof(ICipher))]
    public class TrithemiusCipher : ICipher, ISupportsCustomCharset
    {
        public TrithemiusCipher(string charset = Charsets.English)
        {
            this.Charset = charset;
            _vigenère = new VigenèreCipher(this.Charset);
        }

        private readonly VigenèreCipher _vigenère;

        public string Charset { get; private set; }


        public string Encrypt(string plaintext)
        {
            return _vigenère.Encrypt(plaintext, this.Charset);
        }

        public string Decrypt(string ciphertext)
        {
            return _vigenère.Decrypt(ciphertext, this.Charset);
        }
    }
}
