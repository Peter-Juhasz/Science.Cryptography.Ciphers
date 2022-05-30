# Analyze ciphertext

## Frequency Analysis

### Analyze all characters
You can analyze the frequency of characters:
```cs
AbsoluteCharacterFrequencies frequencies = FrequencyAnalysis.Analyze("Hello world!");
```

This would give the following result:
```
H	1
e	1
l	3
o	2
	1
w	1
r	1
d	1
!	1
```

### Analyze letters only
You can analyze letters only:
```cs
var frequencies = FrequencyAnalysis.AnalyzeLetters("Hello world!");
```

Which would count letters only:
```
H	1
E	1
L	3
O	2
W	1
R	1
D	1
```

### Analyze characters in an alphabet only
Or even specific to an alphabet:
```cs
var frequencies = FrequencyAnalysis.Analyze("Hello world!", WellKnownAlphabets.English);
```

### Memory management
All the previous examples would allocate a new buffer for each analysis cycle. But memory management can be handled by the caller for better performance. For example, the following method provides a fast path for both memory management and ASCII letters:
```cs
Dictionary<char, int> buffer; // preallocated with capacity of 26

ReadOnlySpan<char> text = "Hello world!";
buffer.Clear();
FrequencyAnalysis.AnalyzeAsciiLetters(text, buffer);
```

### Compare to reference
Analysis result is an instance of [`AbsoluteCharacterFrequencies`](../../src/Science.Cryptography.Ciphers.Analysis/AbsoluteCharacterFrequencies.cs), which can be converted to [`RelativeCharacterFrequencies`](../../src/Science.Cryptography.Ciphers.Analysis/RelativeCharacterFrequencies.cs) for comparison:
```cs
AbsoluteCharacterFrequencies absolute = FrequencyAnalysis.Analyze("Hello world!");
RelativeCharacterFrequencies relative = absolute.ToRelativeFrequencies();
```

Then you can compare the frequencies with the reference data from languages:
```cs
var actual = FrequencyAnalysis.Analyze("Hello world!");

var reference = Languages.FromCultureInfo("en-us").RelativeFrequenciesOfLetters;
var score = FrequencyAnalysis.Compare(reference, actual);
```

For better memory management, `FrequencyAnalysis` offers comparison of relative reference data to absolute measurements as well, and can operate on raw dictionaries as well.


## N-Gram Analysis

### Read n-grams
You can read all n-grams like this:
```cs
var ngrams = NGramAnalysis.Read("Hello world!", 2);
foreach (StringSegment segment in ngrams)
{
	// ...
}
```

*Note: for better memory management, `Read` uses a value type enumerator and returns only `StringSegment`s, instead of materialized `string`s (like [`StringTokenizer`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.primitives.stringtokenizer)). So, in this example, there wasn't any heap allocation.*

### Analyze all characters
You can analyze the frequency of characters:
```cs
AbsoluteStringFrequencies frequencies = NGramAnalysis.Analyze("Hello world!", 2);
```

This would give the following result:
```
He	1
el	1
ll	1
lo	1
o 	1
 w
wo	1
or	1
rd	1
d!	1
```

### Analyze letters only
You can analyze the frequency of characters:
```cs
AbsoluteStringFrequencies frequencies = NGramAnalysis.AnalyzeLetters("Hello world!", 2);
```

This would give the following result:
```
HE	1
EL	1
LL	1
LO	1
WO	1
OR	1
RD	1
```


## Kasiki Examination
TODO


## Entropy
TODO


## Score speculative plaintexts
TODO