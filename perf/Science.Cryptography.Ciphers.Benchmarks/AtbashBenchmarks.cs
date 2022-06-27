using BenchmarkDotNet.Attributes;

using Science.Cryptography.Ciphers.Specialized;
using System.Collections.Generic;
using System.Linq;
using System;
using Science.Cryptography.Ciphers;

[MemoryDiagnoser]
public class AtbashBenchmarks
{
	private static readonly AtbashCipher General = new();
	private static readonly AsciiAtbashCipher Optimized = new();

	private static readonly char[] Input = new char[43];
	private static readonly char[] Output = new char[43];

	[Benchmark]
	public void Atbash()
	{
		General.Encrypt(Input, Output, out _);
	}

	[Benchmark]
	public void SlowXor_I64_K32()
	{
		Optimized.Encrypt(Input, Output, out _);
	}
}