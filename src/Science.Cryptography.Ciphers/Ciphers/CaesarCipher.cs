using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents Julis Caesar's cipher.
    /// </summary>
    [Export("Caesar", typeof(ICipher))]
    public class CaesarCipher : ICipher, ISupportsCustomCharset
    {
        public CaesarCipher(string charset = Charsets.English)
        {
            this.Charset = charset;
            _shift = new ShiftCipher(this.Charset);
        }

        private readonly ShiftCipher _shift;

        public string Charset { get; private set; }


        public string Encrypt(string plaintext)
        {
            return _shift.Encrypt(plaintext, WellKnownShiftCipherKeys.Caesar);
        }

        public string Decrypt(string ciphertext)
        {
            return _shift.Decrypt(ciphertext, WellKnownShiftCipherKeys.Caesar);
        }
    }
}
