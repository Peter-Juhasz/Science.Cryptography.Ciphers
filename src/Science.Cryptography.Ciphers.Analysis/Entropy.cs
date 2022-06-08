using System;

namespace Science.Cryptography.Ciphers.Analysis;

public static class Entropy
{
	/// <summary>
	/// Calculates the entropy of <paramref name="input" />.
	/// </summary>
	/// <param name="input"></param>
	/// <param name="logarithmBase"></param>
	/// <returns></returns>
	public static double Analyze(ReadOnlySpan<char> input, double logarithmBase = 2)
	{
		var length = input.Length;
		var sum = 0D;

		foreach (var kv in FrequencyAnalysis.Analyze(input))
		{
			var occurrence = kv.Value;
			var probability = occurrence / length;
			sum -= probability * Math.Log(probability, logarithmBase);
		}

		return sum;
	}
}
