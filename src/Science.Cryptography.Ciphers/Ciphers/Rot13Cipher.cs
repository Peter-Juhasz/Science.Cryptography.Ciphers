using System;
using System.Composition;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the ROT-13 cipher.
    /// </summary>
    [Export("ROT-13", typeof(ICipher))]
    public class Rot13Cipher : ReciprocalCipher, ISupportsCustomCharset
    {
        public Rot13Cipher(string charset = Charsets.English)
        {
            this.Charset = charset;
            _shift = new ShiftCipher(this.Charset);
        }

        private readonly ShiftCipher _shift;

        public string Charset { get; private set; }


        protected override string Crypt(string text)
        {
            return _shift.Encrypt(text, WellKnownShiftCipherKeys.Rot13);
        }
    }
}
