using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Science.Cryptography.Ciphers.Analysis;

public static class KasiskiExamination
{
	public static IEnumerable<KasiskiExaminationResult> FindRepeatedSegments(
		StringSegment ciphertext,
		int minimumKeyLength = 3,
		StringComparison comparison = StringComparison.OrdinalIgnoreCase,
		CancellationToken cancellationToken = default
	)
	{
		if (minimumKeyLength < 0 || minimumKeyLength > ciphertext.Length)
		{
			throw new ArgumentOutOfRangeException(nameof(minimumKeyLength));
		}

		int maximumKeyLength = ciphertext.Length / 2;
		for (int keyLength = minimumKeyLength; keyLength <= maximumKeyLength; keyLength++)
		{
			cancellationToken.ThrowIfCancellationRequested();

			for (int i = 0; i < ciphertext.Length - 2 * keyLength; i++)
			{
				var substring = ciphertext.Subsegment(i, keyLength);

				foreach (var nextOccurrence in ciphertext.AllIndexesOf(substring, i + 1, comparison))
				{
					if ((nextOccurrence - i) % keyLength != 0)
					{
						continue;
					}

					int length = nextOccurrence - i;
					yield return new(length, new string(substring));
				}
			}
		}
	}

	public static IEnumerable<KasiskiExaminationResult> Analyze(
		ReadOnlySpan<char> ciphertext,
		int minimumKeyLength = 3,
		StringComparison comparison = StringComparison.OrdinalIgnoreCase,
		CancellationToken cancellationToken = default
	)
	{
		throw new NotImplementedException();
		/*
		if (minimumKeyLength < 0 || minimumKeyLength > ciphertext.Length)
		{
			throw new ArgumentOutOfRangeException(nameof(minimumKeyLength));
		}

		int maximumKeyLength = ciphertext.Length / 2;
		for (int keyLength = minimumKeyLength; keyLength <= maximumKeyLength; keyLength++)
		{
			cancellationToken.ThrowIfCancellationRequested();

			for (int i = 0; i < ciphertext.Length - 2 * keyLength; i++)
			{
				var substring = ciphertext.Substring(i, keyLength);

				var enumerator = ciphertext.AllIndexesOf(substring, i + 1, comparison).GetEnumerator();
				while (enumerator.MoveNext())
				{
					var nextOccurrence = enumerator.Current;
					if ((nextOccurrence - i) % keyLength != 0)
					{
						continue;
					}

					int length = nextOccurrence - i;
					yield return new(length, new string(substring));
				}
			}
		}*/
	}
}
