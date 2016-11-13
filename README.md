# Science.Cryptography.Ciphers
Ancient and classic cipher methods and analysis tools implemented in **.NET Standard 1.6**.

```PowerShell
Install-Package Science.Cryptography.Ciphers -Pre
Install-Package Science.Cryptography.Ciphers.Analysis -Pre
```

## Ciphers
```C#
ICipher caesar = ShiftCipher.CreateCaesar(Alphabets.English);
string ciphertext = caesar.Encrypt("Hello world!");
```

Affine, Atbash, Autokey, Bacon, Bifid, Caesar, Four-square, Gronsfeld, Gudhayojya, Kama-Sutra, Monoalphabetic Substitution, Morse Code, Multiplicative, Null, ROT-13, ROT-47, Running Key, Sandorf's, Shift, Tap Code, Trithemius, Two-square, Variant Beaufort, Vatsyayana, Vigenère, XOR

### Streaming support
```C#
IKeyedCipher<string> cipher = new RunningKeyCipher();
IEnumerable<char> ciphertextStream = cipher.Encrypt(plaintextStream);
```

Affine, Atbash, Monoalphabetic Substitution, Multiplicative, ROT-47, Running Key, Shift (Caesar, ROT-13), Vigenère

## Tools
```C#
TabulaRecta tc = new TabulaRecta(Alphabets.English);
```

* Polybius Square
* Straddling Checkerboard
* Tabula Recta

## Analysis
```C#
IReadOnlyDictionary<int, string> result = CaesarBruteforce.Analyze(ciphertext);
```

* Caesar Bruteforce
* Entropy
* Frequency Analysis
* Kasiski Examination
* [Key Finder](https://github.com/Peter-Juhasz/Science.Cryptography.Ciphers/wiki/Find-the-key-for-a-given-ciphertext)
* Key space enumeration (Affine, Shift, Wordlist)
* N-Gram Analysis

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