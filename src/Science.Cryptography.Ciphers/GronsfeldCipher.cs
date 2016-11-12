using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    using GronsfeldKey = IReadOnlyList<int>;

    /// <summary>
    /// Represents the cipher named after Johann Franz Graf Gronsfeld-Bronkhorst.
    /// </summary>
    [Export("Gronsfeld", typeof(IKeyedCipher<>))]
    public class GronsfeldCipher : IKeyedCipher<GronsfeldKey>, ISupportsCustomCharset
    {
        public GronsfeldCipher(string charset = Charsets.English)
        {
            this.Charset = charset;
            _vigenère = new VigenèreCipher(this.Charset);
        }

        private readonly VigenèreCipher _vigenère;

        public string Charset { get; private set; }


        public string Encrypt(string plaintext, GronsfeldKey key)
        {
            return _vigenère.Encrypt(plaintext, GetVigenèreKey(key));
        }

        public string Decrypt(string ciphertext, GronsfeldKey key)
        {
            return _vigenère.Decrypt(ciphertext, GetVigenèreKey(key));
        }


        private string GetVigenèreKey(GronsfeldKey key)
        {
            return new String(
                key.Select(d => this.Charset[d]).ToArray()
            );
        }
    }
}
