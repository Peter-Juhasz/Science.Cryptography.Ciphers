using System.Composition;

namespace Science.Cryptography.Ciphers.Analysis;

[Export("Shift", typeof(IKeySpace<int>))]
public sealed class ShiftKeySpace : IntKeySpace
{
	public ShiftKeySpace(Alphabet alphabet)
		: base(0, alphabet.Length - 1)
	{
		Alphabet = alphabet;
	}

	public Alphabet Alphabet { get; }
}
