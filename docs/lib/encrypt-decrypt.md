# Encrypt and decrypt

## Unkeyed ciphers
All unkeyed ciphers implement the [`ICipher`](../../src/Science.Cryptography.Ciphers/ICipher.cs) interface. You can simply create an instance of a cipher and then call `Encrypt` and `Decrypt`:
```cs
var cipher = new Rot13Cipher();
var plaintext = "Hello world!";
var ciphertext = cipher.Encrypt(plaintext); // "Uryyb jbeyq!"
var decrypted = cipher.Decrypt(ciphertext);
```

For most ciphers, you can also provide an [`Alphabet`](basics.md#alphabet):
```cs
var cipher = new Rot13Cipher(WellKnownAlphabets.English);
```

Or some additional options:
```cs
var options = new MorseCodeOptions(Dot: 'A', Dash: 'B', Delimiter: ' ');
var morse = new MorseCode(options);
```

## Keyed ciphers
All keyed ciphers implement the [`IKeyedCipher<TKey>`](../../src/Science.Cryptography.Ciphers/IKeyedCipher.cs) interface. For encryption and decryption, you also need to provide a key:
```cs
var cipher = new ShiftCipher();
var plaintext = "Hello world!";
var key = 13;
var ciphertext = cipher.Encrypt(plaintext, key); // "Uryyb jbeyq!"
var decrypted = cipher.Decrypt(ciphertext, key);
```

You can also pin a key using the `Pin` method, to use a keyed cipher as a regular one with a specific key:
```cs
var shiftCipher = new ShiftCipher(); // IKeyedCipher<int>
var rot13Cipher = shiftCipher.Pin(13); // ICipher
```

### Helpers
The following helper methods are available to construct keys.

There are some well-known shift keys in [`WellKnownShiftCipherKeys`](../../src/Science.Cryptography.Ciphers/Ciphers/ShiftCipher.Keys.cs):

| Name | Value |
| ---- | ----- |
| Caesar | 3 |
| Rot13 | 13 |

To create a [Polybius Square](basic.md#polybius-square) from a keyword, `CreateFromKeyword` starts to fill a polybius square from the top left corner with the letters of the keyword, deduplicated, and fills the remaining slots with letters of the alphabet:
```cs
var square = PolybiusSquare.CreateFromKeyword("PLAYFAIR", WellKnownAlphabets.EnglishWithoutJ);
```
```
P L A Y F
I R B C D
E G H K M
N O Q S T
U V W X Z
```

There are multiple modes available to create an array of ints from keywords. Find indexes of letters in an alphabet:
```cs
int[] key = IntArrayKey.FromCharIndexesOfAlphabet("CARGO", WellKnownAlphabets.English);
// 3 1 18 7 15
```

Find indexes of letters in an alphabet, and also sort them:
```cs
int[] key = IntArrayKey.FromCharIndexesOfAlphabetSorted("CARGO", WellKnownAlphabets.English);
// 1 3 7 15 18
```

Find indexes of letters in an alphabet, but use sequential order for final result:
```cs
int[] key = IntArrayKey.FromCharIndexesOfAlphabetSequential("CARGO", WellKnownAlphabets.English);
// 2 1 5 3 4
```

## Memory management
While in the previous examples you could see `string` inputs and outputs, those were only made possible by extensions methods to simplify usage. But by default, all ciphers let the caller do memory management, they don't allocate memory for the results. So, to avoid allocations, you can use the raw interface of ciphers like this:
```cs
char[] buffer;
shiftCipher.Decrypt("Uryyb jbeyq!", buffer, 17);
```

Analysis tools can take advantage of this behavior, the Gargage Collector can be freed, instead of having to allocate memory for intermediate results over and over again:
```cs
var cipher = new ShiftCipher();
var ciphertext = "Uryyb jbeyq!";
Span<char> buffer = stackalloc char[ciphertext.Length];

for (int i = 1; i < 26; i++)
{
	cipher.Decrypt("Uryyb jbeyq!", buffer, 17, out var written);
	if (Evaluate(buffer[..written]))
	{
		// found
		return new string(buffer[..written]);
	}
}
```

## Binary ciphers
All ciphers are based on the type `char`, but in some special cases, like ASCII, the size of a `char` is equivalent to a `byte`. In those cases, we can use Hardware Intrinsics, and process the Same Instruction for Multiple Data (SIMD) in a single processor cycle.

An example is the [`AsciiXorCipher`](../../src/Science.Cryptography.Ciphers.Specialized/Optimized/AsciiXorCipher.cs), which converts the inputs into a series of [Vector256&lt;byte&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector256-1)s, and takes advantage of the AVX instruction set extensions to execute the xor:

```cs
Span<byte> inputBytes = stackalloc byte[input.Length];
Encoding.ASCII.GetBytes(input, inputBytes);

Span<byte> outputBytes = stackalloc byte[input.Length];
BinaryXor.Avx2Xor256(inputBytes, outputBytes, key); // intrinsics inside
Encoding.GetChars(outputBytes, output);
```

## Implement a cipher
You have to implemenent either the [`ICipher`](../../src/Science.Cryptography.Ciphers/ICipher.cs) or the [`IKeyedCipher<TKey>`](../../src/Science.Cryptography.Ciphers/IKeyedCipher.cs) interfaces, which both expose two methods:
```cs
public interface ICipher
{
	void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written);

	void Decrypt(ReadOnlySpan<char> ciphertext, Span<char> plaintext, out int written);
}
```

As you can see, under the hood, they use character buffers instead of materialized `String`s better memory management. For details, see [Memory Management](#memory-management).

Parameters:
 - `plaintext` (or `ciphertext`): the input buffer
 - `ciphertext` (or `plaintext`): the output buffer
 - `key`: the key for encryption/decryption (if it is an `IKeyedCipher`)
 - out `written`: the number of characters written to the output buffer

The following example simply copies all characters from input to output without making any modification:
```cs
public void Encrypt(ReadOnlySpan<char> plaintext, Span<char> ciphertext, out int written);
{
	for (int i = 0; i < plaintext.Length; i++)
	{
		ciphertext[i] = plaintext[i]; // transform here
	}

	written = plaintext.Length;
}
```

### Buffer allocation
But how do callers know what is the minimum buffer size required for output? Actually the `ICipher` (and `IKeyedCipher<TKey>`) interface exposes one more member, which can give a hint of what buffer size may would be sufficient:
```cs
public interface ICipher
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	int MaxOutputCharactersPerInputCharacter => 1;
}
```

So, if your cipher might write multiple output characters for a single input character, then you should implement this member too. For example, [`BaconCipher`](../../src/Science.Cryptography.Ciphers/Ciphers/BaconCipher.cs) is one of a kind:
```cs
public class BaconCipher : ICipher
{
	public int MaxOutputCharactersPerInputCharacter => 5; // "C" => "AAABA"

	// ...
}
```

### Add to catalog
If you want to make your cipher discoverable and appear in the catalog of the CLI app, add the [`Export`](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.composition.exportattribute) attribute. You can also name of your cipher, and even publish it under multiple names:
```cs
[Export("Bacon", typeof(ICipher)]
[Export("Francis Bacon", typeof(ICipher)]
public class BaconCipher : ICipher
{
	// ...
}
```