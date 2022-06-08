# Solve a cryptogram

## Cryptogram Solver

### Build a solver
A `CryptogramSolver` must be configured before use, using a `CryptogramSolverBuilder`.

First of all, the an `ISpeculativePlaintextScorer` must be set to score each candidate:
```cs
var scorer = new SubstringSpeculativePlaintextScorer("solution");
var builder = new CryptogramSolverBuilder(scorer);
```

*For more information on scoring, see [Scoring](docs/lib/find-key.md#scoring).*

Then, you have to add ciphers which are going to be tested by the solver:
```cs
builder.AddCihper(new BaconCipher());
```

Ciphers can be added multiple times with different configurations as well:
```cs
builder.AddCihper(new BaconCipher(new BaconOptions(A: 'B', B: 'A')));
```

For keyed ciphers, a key space must be configured:
```cs
builder.AddCipher(new ShiftCipher(), new IntKeySpace(1, 25));
```

The `CryptogramSolver` supports all [kinds of key spaces](docs/lib/find-key.md#kinds-of-key-spaces).

Optionally, you can also provide your own [`ILogger`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger) instance to get logs:
```cs
builder.SetLogger(logger); // ILogger
```

### Run
Full example:
```cs
var ciphertext = "aol zvsbapvu pz jyfwavnyht";
var scorer = new SubstringSpeculativePlaintextScorer("solution");
var promoter = new ProgressivelyBetterCandidatePromoter();

var solver = new CryptogramSolverBuilder(scorer)
	.AddCipher(new BaconCipher())
	.AddCipher(new TapCode())
	.AddCipher(new ShiftCipher(), new ShiftKeySpace())
	// ...
	.Build();

await foreach (var candidate in solver.SolveAsync(ciphertext, promoter))
{
	var cipher = candidate.Cipher.Cipher;
	if (candidate.Cipher.HasKey)
	{
		var key = candidate.Key;
		// ...
	}
	// ...
	var plaintext = candidate.SpeculativePlaintext.Plaintext;
	var score = candidate.SpeculativePlaintext.Score;
	// ...
}
```

### Depth
`CryptogramSolver` supports only a single depth of encryption.