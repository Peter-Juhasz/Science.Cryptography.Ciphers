# Find key

## Introduction
Key space &gt; Speculative plaintext scorer &gt; Candidate promoter &gt; Results

## Key spaces
We need to define the key space to search.

Kinds of key spaces:

| Type | Description |
| ---- | ----------- |
| [IKeySpace&lt;TKey&gt;](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/IKeySpace.cs) | Represents a synchronous, sequential keyspace. |
| [IAsyncKeySpace&lt;TKey&gt;](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/IAsyncKeySpace.cs) | Represents an asynchronous, sequential keyspace. |
| [IPartitionedKeySpace&lt;TKey&gt;](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/IKeySpace.cs) | Represents a synchronous, parallel keyspace. |

The [IKeySpace&lt;TKey&gt;](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/IKeySpace.cs) represents the most basic key space. It is applicable to a small or medium set of values which are either already available in-memory or can be generated synchronously.
```cs
public interface IKeySpace<TKey>
{
	IEnumerable<TKey> GetKeys();
}
```

*See [InMemoryKeySpace&lt;TKey&gt;](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/InMemoryKeySpace.cs) or [IntKeySpace](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/IntKeySpace.cs) for example.*

The [IAsyncKeySpace&lt;TKey&gt;](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/IAsyncKeySpace.cs) represents a key space where values are generated asynchronously. It is applicable to reading data from a file or a remote source.
```cs
public interface IAsyncKeySpace<TKey>
{
	IAsyncEnumerable<TKey> GetKeys();
}
```

*See [FileWordlistKeySpace](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/FileWordlistKeySpace.cs) for example.*

The [IPartitionedKeySpace&lt;TKey&gt;](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/IPartitionedKeySpace.cs) is the most advanced key space. It supports parallel enumeration, by breaking the whole key space into partitions. State can be preserved in each partition to reuse memory and avoid pooling. It is applicable to large key spaces which can be divided into chunks, for example brute force of characters or numbers.
```cs
public interface IPartitionedKeySpace<TKey>
{
	IEnumerable<IKeySpace<TKey>> GetPartitions(int? desiredCount = null);
}
```

*See [ArrayKeySpace&lt;TKey&gt;](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/ArrayKeySpace.cs) for example.*

### Built-in key spaces
List of general use key spaces:

| Type | Description |
| ---- | ----------- |
| [InMemoryKeySpace&lt;TKey&gt;](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/InMemoryKeySpace.cs) | A predefined set of values of type T. |
| [WordlistKeySpace](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/WordlistKeySpace.cs) | A predefined list of words. |
| [FileWordlistKeySpace](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/FileWordlistKeySpace.cs) | Reads words from a file line by line asynchronously. |
| [IntKeySpace](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/IntKeySpace.cs) | Enumerates integers from a minimum to a maximum value. |
| [ArrayKeySpace&lt;TKey&gt;](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/ArrayKeySpace.cs) | Generates variations of a set of values (a.k.a. brute force). Supports partitions for parallel enumeration. |
| [AggregateKeySpace&lt;TKey&gt;](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/AggregateKeySpace.cs) | Aggregates multiple key spaces. |

List of key spaces specific to ciphers:

| Type | Description |
| ---- | ----------- |
| [ShiftKeySpace](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/ShiftKeySpace.cs) | Key space for the Shift cipher. |
| [AffineKeySpace](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/AffineKeySpace.cs) | Key space for the Affine cipher. |
| [NthCharacterKeySpace](../../src/Science.Cryptography.Ciphers.Analysis/KeySpace/NthCharacterKeySpace.cs) | Key space for the Nth character cipher. |

## Scoring
We try to decrypt the ciphertext with each key, but we need to score the results, whether the decrypted text is gibberish or something meaningful.

```cs
public interface ISpeculativePlaintextScorer
{
	double Score(ReadOnlySpan<char> speculativePlaintext);
}
```

List of language statistics based use scorers:

| Type | Description |
| ---- | ----------- |
| [RelativeLetterFrequencies](../../src/Science.Cryptography.Ciphers.Analysis/SpeculativePlaintextScoring/RelativeLetterFrequenciesSpeculativePlaintextScorer.cs) | Compares relative frequencies of letters to reference. |
| [RelativeFirstLetterOfWordsFrequencies](../../src/Science.Cryptography.Ciphers.Analysis/SpeculativePlaintextScoring/RelativeFirstLetterOfWordsFrequenciesSpeculativePlaintextScorer.cs) | Compares relative frequencies of first letters of words to reference. |
| [RelativeNGramFrequencies](../../src/Science.Cryptography.Ciphers.Analysis/SpeculativePlaintextScoring/RelativeNGramFrequenciesSpeculativePlaintextScorer.cs) | Compares relative frequencies of n-grams of letters to reference. |

List of optimized language statistics based use scorers:

| Type | Description |
| ---- | ----------- |
| [AsciiRelativeLetterFrequencies](../../src/Science.Cryptography.Ciphers.Analysis/SpeculativePlaintextScoring/AsciiRelativeLetterFrequenciesSpeculativePlaintextScorer.cs) | Compares relative frequencies of ASCII letters to reference. |
| [TwoGramAsciiLettersRelativeFrequencies](../../src/Science.Cryptography.Ciphers.Analysis/SpeculativePlaintextScoring/TwoGramAsciiLettersRelativeFrequenciesSpeculativePlaintextScorer.cs) | Compares relative frequencies of 2-grams of ASCII letters to reference. |

List of other general use scorers:

| Type | Description |
| ---- | ----------- |
| [Substring](../../src/Science.Cryptography.Ciphers.Analysis/SpeculativePlaintextScoring/SubstringSpeculativePlaintextScorer.cs) | Returns 1 if the specified substring is found, otherwise 0. |
| [AnySubstring](../../src/Science.Cryptography.Ciphers.Analysis/SpeculativePlaintextScoring/AnySubstringSpeculativePlaintextScorer.cs) | Returns 1 if any of the substrings is found, otherwise 0. |
| [Regex](../../src/Science.Cryptography.Ciphers.Analysis/SpeculativePlaintextScoring/RegexSpeculativePlaintextScorer.cs) | Returns 1 if the specified regular expression matches, otherwise 0. |
| [Charset](../../src/Science.Cryptography.Ciphers.Analysis/SpeculativePlaintextScoring/CharsetSpeculativePlaintextScorer.cs) | Returns the percentage of how many of the characters can be found in the specified character set. |
| [Wordlist](../../src/Science.Cryptography.Ciphers.Analysis/SpeculativePlaintextScoring/WordlistSpeculativePlaintextScorer.cs) | Returns the percentage of how many of the words can be found in the specified wordlist. |
| [Aggregate](../../src/Science.Cryptography.Ciphers.Analysis/SpeculativePlaintextScoring/AggregateSpeculativePlaintextScorer.cs) | Aggregates the scores of multiple scorers. |

## Promotion of candidates
All speculative results have a score now, but we still need to decide which ones to select for further/manual analysis.

```cs
public interface ICandidatePromoter
{
	bool Promote(double score);
}
```

List of general use candidate promoters:

| Type | Description |
| ---- | ----------- |
| [AllCandidatePromoter](../../src/Science.Cryptography.Ciphers.Analysis/AllCandidatePromoter.cs) | Promotes all candidates. |
| [OverThresholdCandidatePromoter](../../src/Science.Cryptography.Ciphers.Analysis/OverThresholdCandidatePromoter.cs) | Promotes candidates only which has a score higher than specified. |
| [ProgressivelyBetterCandidatePromoter](../../src/Science.Cryptography.Ciphers.Analysis/ProgressivelyBetterCandidatePromoter.cs) | The all time high is progressively adjusted when a better result is found, and all candidates with lower score than that are dropped. |

## Analysis
Once we have everything set up for a search, we can start the search by supplying the desired configuration to [`KeyFinder`](../../src/Science.Cryptography.Ciphers.Analysis/KeyFinder.cs).

Use `Solve` with a basic key space:
```cs
foreach (var candidate in KeyFinder.Solve(
	"Wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj.",
	new ShiftCipher(),
	new ShiftKeySpace(WellKnownAlphabets.English),
	new SubstringSpeculativePlaintextScorer("quick")
))
{
	var plaintext = candidate.SpeculativePlaintext.Plaintext;
	var score = candidate.SpeculativePlaintext.Score;
	var key = candidate.Key;
	// ...
}
```

Use `SolveParallelAsync` with a parallel key space:
```cs
await foreach (var candidate in KeyFinder.SolveParallelAsync(
	"Wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj.",
	new XorCipher(),
	new ArrayKeySpace(1, 3, Enumerable.Range(32, 96).ToHashSet()),
	new RelativeNGramFrequenciesSpeculativePlaintextScorer(
		Languages.English.GetNGramFrequencies(2)
	),
	new ProgressivelyBetterCandidatePromoter()
))
{
	// ...
}
```

Use `SolveAsync` with an asynchronous key space:
```cs
await foreach (var candidate in KeyFinder.SolveAsync(
	"Wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj.",
	new XorCipher().AsStringKeyed(),
	new FileWordlistKeySpace(@"english.txt"),
	new RelativeNGramFrequenciesSpeculativePlaintextScorer(
		Languages.English.GetNGramFrequencies(2)
	),
	new ProgressivelyBetterCandidatePromoter()
))
{
	// ...
}
```

Solve a specific cipher for best result:
```cs
var best = KeyFinder.SolveForBest(
	"Wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj.",
	new ShiftCipher(),
	new ShiftKeySpace(WellKnownAlphabets.English),
	new SubstringSpeculativePlaintextScorer("quick")
);
```

## Utilities