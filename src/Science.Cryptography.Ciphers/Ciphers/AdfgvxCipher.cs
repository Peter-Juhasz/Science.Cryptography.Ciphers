using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

[Export("ADFGVX", typeof(IKeyedCipher<>))]
public class AdfgvxCipher : AdfgvxBaseCipher
{
	public AdfgvxCipher(AdfgvxCipherOptions options) : base(options)
	{
		if (options.Labels.Length != 6)
		{
			throw new ArgumentException($"The required size of {nameof(options.Labels)} is {6}.", nameof(options));
		}
	}
	public AdfgvxCipher()
		: this(AdfgvxCipherOptions.ADFGVX)
	{ }
}
