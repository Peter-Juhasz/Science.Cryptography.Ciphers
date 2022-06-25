# Science.Cryptography.Ciphers v2 PREVIEW

![NuGet](/Peter-Juhasz/Science.Cryptography.Ciphers/actions/workflows/nuget.yml/badge.svg) ![CLI](/Peter-Juhasz/Science.Cryptography.Ciphers/actions/workflows/cli.yml/badge.svg)

Ancient and classic cipher methods and analysis tools implemented in **.NET** by using high performance memory management and SIMD hardware intrinsics.

Use command-line interface:
```sh
crypto encrypt shift -k 13 "Hello world!"
crypto solve "Wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj."
```

Use rich framework APIs:
```ps
dotnet add package Science.Cryptography.Ciphers
dotnet add package Science.Cryptography.Ciphers.Specialized
dotnet add package Science.Cryptography.Ciphers.Analysis
```

## What's new in v2?
- **New CLI app** for new way of usage
- Ciphers and tools rewritten to **allocation free** operation, take advantage of **hardware intrinsics**, and use specialized **fast path for ASCII** encoding. See [Performance Improvements](docs/performance-improvements.md) for details and benchmarks.
- Reworked analysis tools and `IAsyncEnumerable` interface for consuming analysis intermediate results
- **CryptogramSolver** for automatic decryption of ciphertext
- Brute force key spaces with parallel partitioning support
- New ciphers: Scytale, Morse Code with extended charset, Polybius, Columnar Transposition, Double Columnar Transposition, ADFGX, ADFGVX, One-Time Pad, Wolfenbütteler, Malespin
- More detailed documentation

## Command-line interface
Many of the library operations are published via CLI as well:

 - [List available assets](docs/cli/list.md)
 - [Encrypt and decrypt text](docs/cli/encrypt-decrypt.md)
 - [Analyze ciphertext](docs/cli/analyze.md)
	- [Caesar Brute-force](docs/cli/analyze.md#caesar-brute-force)
	- [Frequency analysis](docs/cli/analyze.md#frequency-analysis)
	- [NGram analysis](docs/cli/analyze.md#ngram-analysis)
	- [Score](docs/cli/analyze.md#score)
 - [Find key for a ciphertext](docs/cli/find-key.md)
 - [Solve a cryptogram](docs/cli/solve.md)

## Framework APIs
You can unlock the full power of the library from code:

```cs
var cipher = new PlayfairCipher();
var key = PolybiusSquare.CreateFromKeyword("PLAYFAIR", WellKnownAlphabets.EnglishWithoutJ);
var ciphertext = cipher.Encrypt("Hello world!", key);
```

 - [Basic tools](docs/lib/basics.md)
	- [Alphabet](docs/lib/basics.md#alphabet)
	- [Tabula Recta](docs/lib/basics.md#tabula-recta)
	- [Polybius Square](docs/lib/basics.md#polybius-square)
 - [Encrypt and decrypt text](docs/lib/encrypt-decrypt.md)
	- [Memory management](docs/lib/encrypt-decrypt.md#memory-management)
 - [Implement a cipher](docs/lib/encrypt-decrypt.md#implement-a-cipher)
 - [Analyze ciphertext](docs/lib/analyze.md)
	- [Caesar Brute-force](docs/lib/analyze.md#caesar-brute-force)
	- [Frequency analysis](docs/lib/analyze.md#frequency-analysis)
	- [NGram analysis](docs/lib/analyze.md#ngram-analysis)
	- [Score](docs/lib/analyze.md#score)
 - [Find key for a ciphertext](docs/lib/find-key.md)
    - [Keyspaces](docs/lib/find-key.md#key-spaces)
    - [KeyFinder](docs/lib/find-key.md#analysis)
 - [Solve a cryptogram](docs/lib/solve.md)

## Assets
The full list of assets:

 - [List of ciphers](docs/assets.md#list-of-ciphers)
 - [List of languages](docs/assets.md#list-of-languages)
 - [List of encodings](docs/assets.md#list-of-encodings)

## Accepting PRs
* Enigma
* Hill
* Permutation
* Rail fence
* Myszkowski Transposition
* Nihilist
* Solitaire
* Trifid
* Any other missing cipher
* Unit tests