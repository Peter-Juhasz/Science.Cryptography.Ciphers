using System.Composition;

namespace Science.Cryptography.Ciphers;

/// <summary>
/// Synonym for <see cref="KamaSutraCipher"/>.
/// </summary>
[Export("Vatsyayana", typeof(IKeyedCipher<>))]
public class VatsyayanaCipher : KamaSutraCipher
{
}
