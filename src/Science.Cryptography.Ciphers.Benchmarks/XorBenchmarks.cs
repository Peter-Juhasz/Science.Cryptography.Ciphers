using BenchmarkDotNet.Attributes;

using Science.Cryptography.Ciphers.Specialized;

[MemoryDiagnoser]
public class XorBenchmarks
{
	public XorBenchmarks()
	{

	}

	private static readonly byte[] Input32 = new byte[32];
	private static readonly byte[] Input48 = new byte[48];
	private static readonly byte[] Input72 = new byte[72];
	private static readonly byte[] Input96 = new byte[96];

	private static readonly byte[] Key7 = new byte[7];
	private static readonly byte[] Key32 = new byte[32];
	private static readonly byte[] Key41 = new byte[41];

	private static readonly byte[] Output96 = new byte[96];


	[Benchmark]
	public void SlowXor_I32_K32()
	{
		BinaryXor.SlowXor(Input32, Output96, Key32);
	}

	[Benchmark]
	public void Avx2Xor_I32_K32()
	{
		BinaryXor.Avx2Xor256(Input32, Output96, Key32);
	}

	[Benchmark]
	public void Avx2Xor_I72_K7()
	{
		BinaryXor.Avx2Xor256(Input72, Output96, Key7);
	}

	[Benchmark]
	public void Avx2Xor_I96_K41()
	{
		BinaryXor.Avx2Xor256(Input96, Output96, Key41);
	}

	[Benchmark]
	public void Avx2Xor_I96_K32()
	{
		BinaryXor.Avx2Xor256(Input96, Output96, Key32);
	}
}