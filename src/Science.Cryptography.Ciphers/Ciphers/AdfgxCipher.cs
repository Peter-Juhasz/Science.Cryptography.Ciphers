using System;
using System.Composition;

namespace Science.Cryptography.Ciphers;

[Export("ADFGX", typeof(IKeyedCipher<>))]
public class AdfgxCipher : AdfgvxBaseCipher
{
	public AdfgxCipher(AdfgvxCipherOptions options) : base(options)
	{
		if (options.Labels.Length != 5)
		{
			throw new ArgumentException($"The required size of {nameof(options.Labels)} is {5}.", nameof(options));
		}
	}
	public AdfgxCipher()
		: this(AdfgvxCipherOptions.ADFGX)
	{ }
}
