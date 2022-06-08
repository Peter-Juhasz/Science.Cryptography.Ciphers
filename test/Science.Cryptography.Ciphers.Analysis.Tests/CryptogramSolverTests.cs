using Microsoft.VisualStudio.TestTools.UnitTesting;
using Science.Cryptography.Ciphers.Specialized;
using System.Threading.Tasks;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class CryptogramSolverTests
{
	[TestMethod]
	public async Task Morse()
	{
		const string ciphertext = "-- --- .-. ... .  -.-. --- -.. .";
		const string plaintext = "MORSE CODE";

		var solver = new CryptogramSolverBuilder(new RelativeLetterFrequenciesSpeculativePlaintextScorer(Languages.English.RelativeFrequenciesOfLetters))
			.AddCipher(new InternationalMorseCode())
			.AddCipher(new TapCode())
			.Build();

		var result = await solver.SolveForBestAsync(ciphertext);
		Assert.IsNotNull(result);
		Assert.AreEqual(result.SpeculativePlaintext.Plaintext, plaintext);
	}

	[TestMethod]
	public async Task Caesar()
	{
		const string ciphertext = "Wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj.";
		const string plaintext = "The quick brown fox jumps over the lazy dog.";

		var solver = new CryptogramSolverBuilder(new TwoGramAsciiLettersRelativeFrequenciesSpeculativePlaintextScorer(Languages.English.GetNGramFrequencies(2)))
			.AddCipher(new ShiftCipher(), new ShiftKeySpace(WellKnownAlphabets.English))
			.Build();

		var result = await solver.SolveForBestAsync(ciphertext);
		Assert.IsNotNull(result);
		Assert.AreEqual(result.SpeculativePlaintext.Plaintext, plaintext);
	}
}

public static class CryptogramSolverExtensions
{
	private static readonly IKeySpace<string> Wordlist = new InMemoryWordlistKeySpace(new[] { "secret", "key" });

	public static CryptogramSolverBuilder AddAllCiphers(this CryptogramSolverBuilder builder) => builder
		.AddCipher(new AtbashCipher())
		.AddCipher(new InternationalMorseCode())
		.AddCipher(new TapCode())
		.AddCipher(new A1Z26Cipher())
		.AddCipher(new Rot13Cipher(WellKnownAlphabets.English))
		.AddCipher(new Rot47Cipher())
		.AddCipher(new Base64Encoder())
		.AddCipher(new HexEncoder())
		.AddCipher(new BaconCipher())
		.AddCipher(new TrithemiusCipher())
		.AddCipher(new ShiftCipher(), new ShiftKeySpace(WellKnownAlphabets.English))
		.AddCipher(new AffineCipher(), new AffineKeySpace(WellKnownAlphabets.English))
		.AddCipher(new VigenèreCipher(), Wordlist)
	;
}
