# Science.Cryptography.Ciphers

This project collects classic cipher implementations in **.NET Standard 1.6**.

## Ciphers
```C#
ICipher caesar = ShiftCipher.CreateCaesar(Alphabets.English);
string ciphertext = caesar.Encrypt("Hello world!");
```

Affine, Atbash, Autokey, Bacon, Bifid, Four-square, Monoalphabetic Substitution, Morse Code, Multiplicative, Null, ROT-47, Running Key, Sandorf's, Shift (Caesar, ROT-13), Tap Code, Two-square, Vigenère, XOR

### Streaming support
```C#
IKeyedCipher<string> cipher = new RunningKeyCipher();
IEnumerable<char> ciphertextStream = cipher.Encrypt(plaintextStream);
```

Affine, Atbash, Monoalphabetic Substitution, Multiplicative, ROT-47, Running Key, Shift (Caesar, ROT-13), Vigenère

### Key space enumeration
Affine, Shift

## Tools
```C#
IReadOnlyDictionary<int, string> result = CaesarBruteforce.Analyze(ciphertext);
```

* Caesar Bruteforce
* Entropy
* Frequency Analysis
* N-Gram Analysis
* Polybius Square
* Straddling Checkerboard
* Tabula Recta

## Reference data
* Relative frequencies of letters (English)
* Relative frequencies of first letters of words (English)

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