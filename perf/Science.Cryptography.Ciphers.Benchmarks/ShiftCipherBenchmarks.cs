using BenchmarkDotNet.Attributes;
using Science.Cryptography.Ciphers;
using Science.Cryptography.Ciphers.Specialized;

[MemoryDiagnoser]
public class ShiftCipherBenchmarks
{
	private static readonly ShiftCipher General = new();
	private static readonly AsciiShiftCipher Optimized = new();

	private static readonly char[] Output = new char[64];

	private const string Plaintext = "The quick brown fox jumps over the lazy dog.";
	private const int Key = 13;

	[Benchmark]
	public void ShiftCipher_General()
	{
		General.Encrypt(Plaintext, Output, Key, out _);
	}

	[Benchmark]
	public void ShiftCipher_Ascii_Avx2128()
	{
		Optimized.Encrypt(Plaintext, Output, Key, out _);
	}
}