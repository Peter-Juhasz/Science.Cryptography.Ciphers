# Science.Cryptography.Ciphers
Ancient and classic cipher methods and analysis tools implemented in **.NET** by using high performance memory management and SIMD hardware intrinsics.

```ps
dotnet add package Science.Cryptography.Ciphers
dotnet add package Science.Cryptography.Ciphers.Specialized
dotnet add package Science.Cryptography.Ciphers.Analysis
```

## Ciphers
```cs
ICipher caesar = ShiftCipher.CreateCaesar(Alphabets.English);
string ciphertext = caesar.Encrypt("Hello world!");
```

Affine, Atbash, Autokey, Bacon, Beaufort, Bifid, Caesar, Four-square, Gronsfeld, Gudhayojya, Kama-Sutra, Monoalphabetic Substitution, Morse Code, Multiplicative, Null, ROT-13, ROT-47, Running Key, Sandorf's, Shift, Tap Code, Trithemius, Two-square, Variant Beaufort, Vatsyayana, Vigenère, XOR

## Tools
```cs
TabulaRecta tc = new TabulaRecta(Alphabets.English);
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

### Streaming support
```cs
IKeyedCipher<string> cipher = new RunningKeyCipher();
IEnumerable<char> ciphertextStream = cipher.Encrypt(plaintextStream);
```

Affine, Atbash, Monoalphabetic Substitution, Multiplicative, ROT-47, Running Key, Shift (Caesar, ROT-13), Vigenère

## Reference data
* English
  * Relative frequencies of letters
  * Relative frequencies of first letters of words

## Tutorials
* [Encrypt and decrypt text](https://github.com/Peter-Juhasz/Science.Cryptography.Ciphers/wiki/Encrypt-and-decrypt-text)
* [Determine the cipher method for a given ciphertext](https://github.com/Peter-Juhasz/Science.Cryptography.Ciphers/wiki/Determine-the-cipher-method-for-a-given-ciphertext)
* [How to choose between speculative plaintext candidates](https://github.com/Peter-Juhasz/Science.Cryptography.Ciphers/wiki/How-to-choose-between-speculative-plaintext-candidates)
* [Decrypt ciphertext encrypted with an unkeyed cipher](https://github.com/Peter-Juhasz/Science.Cryptography.Ciphers/wiki/Decrypt-ciphertext-encrypted-with-an-unkeyed-cipher)
* [Find the key for a given ciphertext](https://github.com/Peter-Juhasz/Science.Cryptography.Ciphers/wiki/Find-the-key-for-a-given-ciphertext)

## Accepting PRs
* ADFGVX
* Columnar Transposition 
* Enigma
* Hill
* Permutation
* Playfair
* Rail fence
* Myszkowski Transposition
* Nihilist
* One Time Pad
* Solitaire
* Trifid
* Any other missing cipher
* Unit tests