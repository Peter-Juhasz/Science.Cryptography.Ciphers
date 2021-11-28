using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Science.Cryptography.Ciphers.Analysis;

public class CryptogramSolver
{
    private readonly List<ICipher> _ciphers;
    private readonly List<(object cipher, object source)> _keyedCiphers;

    public void AddCipher(ICipher cipher)
    {
        _ciphers.Add(cipher);
    }

    public void AddCipher<TKey>(IKeyedCipher<TKey> cipher, IKeySpaceSource<TKey> source)
    {
        _keyedCiphers.Add((cipher, source));
    }

    public async Task SolveAsync(string ciphertext, CancellationToken cancellationToken)
    {
        var buffer = new char[ciphertext.Length];
        int written;

        foreach (var cipher in _ciphers)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var desiredSize = cipher.MaxOutputCharactersPerInputCharacter * ciphertext.Length;
            if (desiredSize > buffer.Length)
            {
                Array.Resize(ref buffer, desiredSize);
            }

            try
            {
                cipher.Decrypt(ciphertext, buffer, out written);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
