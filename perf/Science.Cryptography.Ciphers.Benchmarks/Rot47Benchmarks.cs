using BenchmarkDotNet.Attributes;

using Science.Cryptography.Ciphers.Specialized;
using System.Collections.Generic;
using System.Linq;
using System;
using Science.Cryptography.Ciphers;

[MemoryDiagnoser]
public class Rot47Benchmarks
{
	private static readonly Rot47Cipher General = new();
	private static readonly AsciiRot47Cipher Optimized = new();

	private const string Plaintext = "The quick brown fox jumps over the lazy dog.";
	private static readonly char[] Output = new char[64];

	[Benchmark]
	public void Atbash()
	{
		General.Encrypt(Plaintext, Output, out _);
	}

	[Benchmark]
	public void SlowXor_I64_K32()
	{
		Optimized.Encrypt(Plaintext, Output, out _);
	}
}