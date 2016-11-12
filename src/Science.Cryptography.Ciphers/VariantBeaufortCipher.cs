using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the Variant Beaufort.
    /// </summary>
    [Export("Variant Beaufort", typeof(IKeyedCipher<>))]
    public class VariantBeaufortCipher : IKeyedCipher<string>
    {
        public VariantBeaufortCipher(string charset = Charsets.English)
        {
            this.Charset = charset;
            _vigenère = new VigenèreCipher(this.Charset);
        }

        private readonly VigenèreCipher _vigenère;

        public string Charset { get; private set; }


        public string Encrypt(string plaintext, string key)
        {
            return _vigenère.Decrypt(plaintext, key);
        }

        public string Decrypt(string ciphertext, string key)
        {
            return _vigenère.Encrypt(ciphertext, key);
        }
    }
}
