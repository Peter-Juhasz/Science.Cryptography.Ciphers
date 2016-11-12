using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Science.Cryptography.Ciphers
{
    /// <summary>
    /// Represents the XOR cipher.
    /// </summary>
    [Export("XOR", typeof(IKeyedCipher<>))]
    public class XorCipher : ReciprocalKeyedCipher<IReadOnlyList<byte>>
    {
        protected override string Crypt(string text, IReadOnlyList<byte> key)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (key.Count == 0)
                throw new ArgumentException("Key can't be zero-length.", nameof(key));

            return String.Concat(
                text.Zip(EnumerableEx.Repeat<byte>(key), (c, k) => (char)(c ^ k))
            );
        }
    }
}
