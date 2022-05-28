# Science.Cryptography.Ciphers
Ancient and classic cipher methods and analysis tools implemented in **.NET** by using high performance memory management and SIMD hardware intrinsics.

Use command-line interface:

```sh
crypto solve "Wkh txlfn eurzq ira mxpsv ryhu wkh odcb grj."
```

Use rich APIs:

```ps
dotnet add package Science.Cryptography.Ciphers
dotnet add package Science.Cryptography.Ciphers.Specialized
dotnet add package Science.Cryptography.Ciphers.Analysis
```

## What's new in v2?
- **New CLI app** for regular users
- Ciphers and tools rewritten to **allocation free** operation, take advantage of **hardware intrinsics**, and specialized **fast path for ASCII** encoding. See [Performance Improvements](docs/performance-improvements.md) for details and benchmarks.
- Reworked analysis tools and `IAsyncEnumerable` interface for consuming analysis intermediate results
- **CryptogramSolver** for automatic decryption of ciphertext
- New brute-force key spaces
- New ciphers: Morse Code with extended charset, Polybius, One-Time Pad, Wolfenbütteler, Malespin

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

 - [Basic tools](docs/lib/basics.md)
	- [Alphabet](docs/lib/basics.md#alphabet)
	- [Tabula Recta](docs/lib/basics.md#tabula-recta)
	- [Polybius Square](docs/lib/basics.md#polybius-square)
 - [Encrypt and decrypt text](docs/lib/encrypt-decrypt.md)
	- [Keyed ciphers](docs/lib/keyed-ciphers.md)
	- [High performance](docs/lib/keyed-ciphers.md)
 - [Analyze ciphertext](docs/lib/analyze.md)
	- [Caesar Brute-force](docs/lib/analyze.md#caesar-brute-force)
	- [Frequency analysis](docs/lib/analyze.md#frequency-analysis)
	- [NGram analysis](docs/lib/analyze.md#ngram-analysis)
	- [Score](docs/lib/analyze.md#score)
 - [Find key for a ciphertext](docs/lib/find-key.md)
    - [Keyspaces](docs/lib/find-key.md#keyspaces)
    - [KeyFinder](docs/lib/find-key.md#keyfinder)
 - [Solve a cryptogram](docs/lib/solve.md)

## Ciphers
```cs
ICipher caesar = new CaesarCipher();
string ciphertext = caesar.Encrypt("Hello world!");
```

Affine, Atbash, Autokey, Bacon, Beaufort, Bifid, Caesar, Four-square, Gronsfeld, Gudhayojya, Kama-Sutra, Monoalphabetic Substitution, Morse Code, Multiplicative, Null, Playfair, ROT-13, ROT-47, Running Key, Sandorf's, Shift, Tap Code, Trithemius, Two-square, Variant Beaufort, Vatsyayana, Vigenère, XOR

## Tools
```cs
var tabula = new TabulaRecta(Alphabets.English);
var square = PolybiusSquare.FromKeyword("EXAMPLE", WellKnownAlphabets.EnglishWithoutQ);
```

* Polybius Square
* Straddling Checkerboard
* Tabula Recta

## Analysis
```cs
IReadOnlyDictionary<int, string> result = CaesarBruteforce.Analyze(ciphertext);
```

* Caesar Bruteforce
* Entropy
* Frequency Analysis
* Kasiski Examination
* [Key Finder](https://github.com/Peter-Juhasz/Science.Cryptography.Ciphers/wiki/Find-the-key-for-a-given-ciphertext)
* Key space enumeration (Affine, Shift, Wordlist)
* N-Gram Analysis

### High-performance support
Where applicable, an ASCII optimized version may be available, so the cost of encoding can be eliminated and data can be manipulated as raw bytes:
```cs
var cipher = new AsciiHexCipher();
var result = cipher.Encrypt("thequickbrownfoxjumpsoverthelazydog");
```

In some cases, the binary implementation is exposed, like `BinaryXor` for `AsciiXorCipher` which operates with raw bytes:
```cs
BinaryXor.Xor(inputBytes, outputBytes, keyBytes);
```

The implementation of `BinaryXor` not only uses no heap allocation, but it uses vectorized SIMD instructions via hardware intrinsics to speed up computations.

## Accepting PRs
* ADFGVX
* Columnar Transposition 
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