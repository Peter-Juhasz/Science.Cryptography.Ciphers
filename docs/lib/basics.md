# Basics

## Alphabet
An alphabet is the most primitive building block of almost all cryptography operations. An alphabet is an ordered list of letters, which usually determines the domain of the encryption.

### Create an alphabet
You can create one by simplfy specifiying the exact characters:
```cs
var alphabet = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
```

Or reorder letters by a keyword:
```cs
var english = new Alphabet("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
var key = Alphabet.FromKeyword("EXAMPLE", english); // "EXAMPLBCDFGHIJKNOQRSTUVWYZ"
```

The number of characters can be found in `Length`:
```cs
int count = alphabet.Length; // 26
```

### Access characters in an alphabet
*Note: indexing starts from `0`.*

Simply index the alphabet:
```cs
char thirdLetter = alphabet[2]; // 'C'
```

Index either from the start or its end:
```cs
char lastLetter = alphabet[^1]; // 'Z'
```

Or get a range:
```cs
ReadOnlySpan<char> fromSecondToFourth = alphabet[1..5]; // "BCD"
```

### Operations
You can access characters by indexing:
```cs
char thirdLetter = alphabet[2]; // 'C'
```

But many ciphers need arithmetic operations which can under- or overflow, so alphabet provides `AtMod` for convenient out of bound access:
```cs
char nextSecondLetter = alphabet.AtMod(27); // 'B'
char lastLetter = alphabet.AtMod(-1); // 'Z'
```

*Note: all other indexing operations throw an `ArgumentOutOfRangeException` in case of overflow.*

Check whether a character is in the alphabet:
```cs
bool containsG = alphabet.Contains('G'); // true
bool containsg = alphabet.Contains('g', StringComparison.OrdinalIgnoreCase); // true

bool containsOmega = alphabet.Contains('Ω'); // false
```

Find the index of a character in the alphabet:
```cs
int indexOfE = alphabet.IndexOf('E'); // 4
int indexOfe = alphabet.IndexOf('e', StringComparison.OrdinalIgnoreCase); // 4
int indexOfEOre = alphabet.IndexOfIgnoreCase('E'); // 4
```

### Extract its content
You can access it as a string:
```cs
string str = alphabet.ToString();
```

You can convert it to a character array:
```cs
char[] array = alphabet.ToCharArray();
```

*Warning: `ToCharArray` allocates a new array for each call*

If you want to spare memory allocation, you can either read the alphabet as a span:
```cs
ReadOnlySpan<char> characters = alphabet.AsSpan();
```

Or copy its content to a preallocated buffer:
```cs
char[] buffer;
alphabet.CopyTo(buffer);
```

### Well known alphabets
There are a couple of useful alphabets built in, which can be access from the `WellKnownAlphabets` class:
```cs
var english = WellKnownAlphabets.English;
```

List of built-in alphabets:

| Name | Letters |
| ---- | ------- |
| `English` | `ABCDEFGHIJKLMNOPQRSTUVWXYZ` |
| `EnglishWithoutI` | `ABCDEFGHJKLMNOPQRSTUVWXYZ` |
| `EnglishWithoutJ` | `ABCDEFGHIKLMNOPQRSTUVWXYZ` |
| `EnglishWithoutK` | `ABCDEFGHIJLMNOPQRSTUVWXYZ` |
| `EnglishWithoutL` | `ABCDEFGHIJKMNOPQRSTUVWXYZ` |
| `EnglishWithoutQ` | `ABCDEFGHIJKLMNOPRSTUVWXYZ` |

### Use an alphabet
Many ciphers use alphabets, for example:
```cs
var cipher = new Rot13Cipher(alphabet);
```

And many other tools, like [Tabula Recta](#tabula-recta).

## Tabula Recta

The tabula recta is a square table of alphabets, each row of which is made by shifting the previous one to the left (see [Wikipedia](https://en.wikipedia.org/wiki/Tabula_recta)). For example, it is used by [Vigenère](../../src/Science.Cryptography.Ciphers/Ciphers/VigenèreCipher.cs).

### Create a tabula recta
You can create a `TabulaRecta` from an `Alphabet`:
```cs
var tabula = new TabulaRecta(WellKnownAlphabets.English);
```

Which would look like this:
```
  | A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
--+----------------------------------------------------
A | A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
B | B C D E F G H I J K L M N O P Q R S T U V W X Y Z A
C | C D E F G H I J K L M N O P Q R S T U V W X Y Z A B
D | D E F G H I J K L M N O P Q R S T U V W X Y Z A B C
E | E F G H I J K L M N O P Q R S T U V W X Y Z A B C D
F | F G H I J K L M N O P Q R S T U V W X Y Z A B C D E
G | G H I J K L M N O P Q R S T U V W X Y Z A B C D E F
H | H I J K L M N O P Q R S T U V W X Y Z A B C D E F G
I | I J K L M N O P Q R S T U V W X Y Z A B C D E F G H
J | J K L M N O P Q R S T U V W X Y Z A B C D E F G H I
K | K L M N O P Q R S T U V W X Y Z A B C D E F G H I J
L | L M N O P Q R S T U V W X Y Z A B C D E F G H I J K
M | M N O P Q R S T U V W X Y Z A B C D E F G H I J K L
N | N O P Q R S T U V W X Y Z A B C D E F G H I J K L M
O | O P Q R S T U V W X Y Z A B C D E F G H I J K L M N
P | P Q R S T U V W X Y Z A B C D E F G H I J K L M N O
Q | Q R S T U V W X Y Z A B C D E F G H I J K L M N O P
R | R S T U V W X Y Z A B C D E F G H I J K L M N O P Q
S | S T U V W X Y Z A B C D E F G H I J K L M N O P Q R
T | T U V W X Y Z A B C D E F G H I J K L M N O P Q R S
U | U V W X Y Z A B C D E F G H I J K L M N O P Q R S T
V | V W X Y Z A B C D E F G H I J K L M N O P Q R S T U
W | W X Y Z A B C D E F G H I J K L M N O P Q R S T U V
X | X Y Z A B C D E F G H I J K L M N O P Q R S T U V W
Y | Y Z A B C D E F G H I J K L M N O P Q R S T U V W X
Z | Z A B C D E F G H I J K L M N O P Q R S T U V W X Y
```

### Using the tabula recta
You can find intersecting characters by simply indexing it. All operations can be indexed by both numbers or characters.
```cs
char intersection = tabulaRecta['D', 'B']; // 'E'
char intersection = tabulaRecta[3, 1]; // 'E'
```

You can also extract a whole row or column:
```cs
string thirdRow = tabulaRecta.GetRowOrColumn('C'); // "CDEFGHIJKLMNOPQRSTUVWXYZAB"
string thirdRow = tabulaRecta.GetRowOrColumn(2); // "CDEFGHIJKLMNOPQRSTUVWXYZAB"
```

*Warning: instance method `GetRowOrColumn` a new `string`. And a backing storage array `string[]` as well to cache rows, so all subsequent calls can be allocation free.*

To avoid memory allocation, all operations are available as static methods, for example:
```cs
char[] buffer;
TabulaRecta.GetRowOrColumn(alphabet, 'C', buffer); // "CDEFGHIJKLMNOPQRSTUVWXYZAB"
```

## Polybius Square

A Polybius Square is a set of characters ordered into a square matrix. It is used by many ciphers, for example [Playfair](../../src/Science.Cryptography.Ciphers/Ciphers/PlayfairCipher.cs).

### Create a polybius square
You can simply create one by specifying the exact characters (or an alphabet):
```cs
var polybiusSquare = PolybiusSquare.FromCharacters("ABCDEFGHJKLMNOPQRSTUVWXYZ");
```

Which would result in this:
```
A B C D E
F G H I K
L M N O P
Q R S T U
V W X Y Z
```

Or by a keyword:
```cs
var polybiusSquare = PolybiusSquare.FromKeyword("PLAYFAIREXAMPLE", WellKnownAlphabets.EnglishWithoutJ);
```

Which would result in this:
```
P L A Y F
I R E X M
B C D G H
K N O Q S
T U V W Z
```

Once created you can get its size:
```cs
int size = polybiusSquare.Size; // 5
```

### Operations
**Important!** Instead of [x, y] indexing order, use [row, column], see [Layout of two-dimensional arrays](#Layout-of-two-dimensional-arrays) for more information.

Reference for examples:
```
P L A Y F
I R E X M
B C D G H
K N O Q S
T U V W Z
```

You can get characters from it by indexing:
```cs
char secondRowThirdColumn = polybiusSquare[1, 2]; // E
char secondRowThirdColumn = polybiusSquare[(1, 2)]; // E
```

You can test for whether it contains a specific character or not:
```cs
bool containsI = polybiusSquare.Contains('I'); // true
bool containsJ = polybiusSquare.Contains('J'); // false
```

You can find the position of an exact character:
```cs
if (polybiusSquare.TryFindOffsets('I', out (int row, int column) position)
{
	// found
}
```

### Extract its content
You can convert it to a character array:
```cs
char[,] array = polybiusSquare.ToCharArray();
```

*Warning: `ToCharArray` allocates a new array for each call*

### Use a polybius square
Many ciphers use it as a key for example:
```cs
var playfair = new PlayfairCipher();
var key = PolybiusSquare.FromKeyword("PLAYFAIREXAMPLE", WellKnownAlphabets.EnglishWithoutJ);
var plaintext = "sample";
var ciphertext = playfair.Encrypt(plaintext, key);
```


## Arrays

### Layout of two-dimensional arrays
The library follows the layout of arrays determined by the .NET specifications. Which means that two-dimensional arrays are indexed as follows:
1. row index
2. column index

For example, the array:
```
|   | 0 | 1 | 2 |
|---+---+---+---|
| 0 | C | I | P |
|---+---+---+---|
| 1 | H | E | R |
```

has 2 rows and 3 columns, and is indexed as follows:
```cs
example[0, 0] = 'C';
example[0, 1] = 'I';
example[0, 2] = 'P';
example[1, 0] = 'H';
example[1, 1] = 'E';
example[1, 2] = 'R';
```

Its raw memory representation looks like this (2x3 = 6 length):
```
| 0 | 1 | 2 | 3 | 4 | 5 |
|---+---+---+---+---+---|
| C | I | P | H | E | R |
```

The reason the library follows the .NET layout and not the mathematical one (x, y), is to be able to compatible, so a two-dimensional array can be flatten out, to be able to use vectorization techniques (search, copy, etc...).

For example, the [`ArrayHelper`](../../src/Science.Cryptography.Ciphers/Tools/ArrayHelper.cs) class contains lots of utilities to manage arrays. One of them is [`FillFast`](../../src/Science.Cryptography.Ciphers/Tools/ArrayHelper.cs) which basically works like this:
```cs
char[,] buffer = new char[5, 5];
Span<char> flat = MemoryMarshal.CreateSpan(ref buffer[0, 0], length: 25);
alphabet.AsSpan().CopyTo(flat);
```

So we get this:
```
A B C D E
F G H I K
L M N O P
Q R S T U
V W X Y Z
```

And as you can see, the implementation doesn't contain two embedded loops for indexing, it works on the multi-dimensional array as a flat structure, and takes advantages of vectorized operations.