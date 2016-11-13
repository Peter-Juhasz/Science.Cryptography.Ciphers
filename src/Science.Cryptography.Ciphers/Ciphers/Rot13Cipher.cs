using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the ROT-13 cipher.
    /// </summary>
    [Export("ROT-13", typeof(ICipher))]
    public class Rot13Cipher : ICipher, ISupportsCustomCharset
    {
        public Rot13Cipher(string charset = Charsets.English)
        {
            this.Charset = charset;
            _shift = new ShiftCipher(this.Charset);
        }

        private readonly ShiftCipher _shift;

        public string Charset { get; private set; }


        public string Encrypt(string plaintext)
        {
            return _shift.Encrypt(plaintext, WellKnownShiftCipherKeys.Rot13);
        }

        public string Decrypt(string ciphertext)
        {
            return _shift.Decrypt(ciphertext, WellKnownShiftCipherKeys.Rot13);
        }
    }
}
