using BenchmarkDotNet.Attributes;

using Science.Cryptography.Ciphers.Specialized;
using System.Collections.Generic;
using System.Linq;
using System;

[MemoryDiagnoser]
public class V1V2XorBenchmarks
{
	private static readonly byte[] Input43 = new byte[43];
	private static readonly byte[] Input64 = new byte[32];
	private static readonly byte[] Key32 = new byte[32];
	private static readonly byte[] Output64 = new byte[64];

	private static readonly string Text = "the quick brown fox jumps over the lazy dog";
	private static readonly string Input64Text = "the quick brown fox jumps over the lazy dog the quick brown fox ";


	[Benchmark]
	public void V1Xor_I64_K32()
	{
		Crypt(Input64Text, Input64);
	}

	[Benchmark]
	public void SlowXor_I64_K32()
	{
		BinaryXor.SlowXor(Input64, Output64, Key32);
	}

	[Benchmark]
	public void Avx2Xor_I43_K32()
	{
		BinaryXor.Avx2Xor256(Input43, Output64, Key32);
	}

	[Benchmark]
	public void Avx2Xor_I64_K32()
	{
		BinaryXor.Avx2Xor256(Input64, Output64, Key32);
	}

	protected string Crypt(string text, IReadOnlyList<byte> key)
	{
		if (text == null)
			throw new ArgumentNullException(nameof(text));

		if (key == null)
			throw new ArgumentNullException(nameof(key));

		if (key.Count == 0)
			throw new ArgumentException("Key can't be zero-length.", nameof(key));

		return String.Concat(
			text.Zip(EnumerableEx.Repeat<byte>(key), (c, k) => (char)(c ^ k))
		);
	}
}