using BenchmarkDotNet.Attributes;

using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;
using Science.Cryptography.Ciphers.Analysis;
using Science.Cryptography.Ciphers;
using System.Composition;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

[MemoryDiagnoser]
public class V1V2CaesarBruteforceBenchmarks
{
	private static readonly string[] _buffer = new string[26];
	private static readonly Science.Cryptography.Ciphers.ShiftCipher _cipher = new(WellKnownAlphabets.English);
	private static readonly string Text = "the quick brown fox jumps over the lazy dog";


	[Benchmark]
	public void V1()
	{
		Analyze(Text);
	}

	[Benchmark]
	public void V2()
	{
		CaesarBruteforce.Analyze(Text, WellKnownAlphabets.English);
	}

	[Benchmark] 
	public void V2_Optimized()
	{
		CaesarBruteforce.Analyze(Text, _cipher, _buffer);
	}

	#region V1
	private static IReadOnlyDictionary<int, string> Analyze(string text, string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
	{
		if (text == null)
			throw new ArgumentNullException(nameof(text));

		if (charset == null)
			throw new ArgumentNullException(nameof(charset));


		var cipher = new ShiftCipher(charset);

		return Enumerable.Range(0, charset.Length)
			.ToDictionary(k => k, k => cipher.Encrypt(text, k))
		;
	}

	private class ShiftCipher
	{
		public ShiftCipher(string charset)
		{
			if (charset == null)
				throw new ArgumentNullException(nameof(charset));

			this.Charset = charset;
		}

		public string Charset { get; set; }

		protected string Crypt(string text, int key)
		{
			char[] result = new char[text.Length];

			for (int i = 0; i < text.Length; i++)
			{
				int idx = IndexOfIgnoreCase(Charset, text[i]);

				result[i] = idx != -1
					? At(this.Charset, idx + key).ToSameCaseAs(text[i])
					: text[i]
				;
			}

			return new String(result);
		}

		public string Encrypt(string plaintext, int key)
		{
			return this.Crypt(plaintext, key);
		}

		public string Decrypt(string ciphertext, int key)
		{
			return this.Crypt(ciphertext, -key);
		}


		public static int IndexOfIgnoreCase(string source, char subject)
		{
			Char toCompare = subject.ToUpper();

			for (int i = 0; i < source.Length; i++)
			{
				if (source[i].ToUpper() == toCompare)
					return i;
			}

			return -1;
		}

		public static char At(string source, int index)
		{
			return source[Mod(index, source.Length)];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Mod(int a, int b)
		{
			return a >= 0 ? a % b : (b + a) % b;
		}
	}
	#endregion
}