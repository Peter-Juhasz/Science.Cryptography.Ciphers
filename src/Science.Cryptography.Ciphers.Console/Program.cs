using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Science.Cryptography.Ciphers;
using Science.Cryptography.Ciphers.Analysis;
using Science.Cryptography.Ciphers.Console;
using Science.Cryptography.Ciphers.Specialized;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Composition;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

// logging
ILogger logger = new RawConsoleLogger();

// CLI
var rootCommand = new RootCommand("Cryptanalysis toolkit built in .NET");
var cipherArgument = new Argument<string>("cipher", "Specify the cipher.") { Arity = ArgumentArity.ExactlyOne };
var keyArgument = new Option<string?>("--key", "Specify the key for the selected cipher.") { Arity = ArgumentArity.ZeroOrOne };
keyArgument.AddAlias("-k");
var textArgument = new Argument<string>("text", "Specify the text for analysis.") { Arity = ArgumentArity.ExactlyOne };
var simpleDetailedOption = new Option<bool>("--detailed", "Show detailed output.");
simpleDetailedOption.SetDefaultValue(false);
var analysisDetailedOption = new Option<bool>("--detailed", "Show detailed output.");
analysisDetailedOption.SetDefaultValue(true);
var languageOption = new Option<string>("--language", "Specify the language for analysis. Sets alphabet as well.");
languageOption.AddAlias("-l");
languageOption.SetDefaultValue("en");
var alphabetOption = new Option<string?>("--alphabet", "Specify the alphabet for analysis.");
alphabetOption.AddAlias("-a");
var encodingOption = new Option<string?>("--encoding", "Specify the character encoding for some operations.");
encodingOption.AddAlias("-e");
encodingOption.SetDefaultValue("us-ascii");

// list
var listCommand = new Command("list", "List assets.");
rootCommand.AddCommand(listCommand);

// list ciphers
var listCiphersCommand = new Command("ciphers", "List supported ciphers.");
var keyedCiphersOnlyOption = new Option<bool?>("--keyed", "Filter to keyed or unkeyed ciphers only.");
listCiphersCommand.SetHandler((bool? keyedFilter) =>
{
	foreach (var type in CipherCatalog.GetTypes()
		.Where(t => keyedFilter switch
		{
			true => t.GetInterface("IKeyedCipher`1") is not null,
			false => t.GetInterface("IKeyedCipher`1") is null,
			null => true
		})
	)
	{
		var shortNames = String.Join(", ", type.GetCustomAttributes<ExportAttribute>().Select(e => e.ContractName).Distinct(StringComparer.OrdinalIgnoreCase));
		Console.WriteLine($"{type.Name,-32}\t{shortNames}");
	}
}, keyedCiphersOnlyOption);
listCiphersCommand.AddOption(keyedCiphersOnlyOption);
listCommand.AddCommand(listCiphersCommand);

// list languages
var listLanguagesCommand = new Command("languages", "List supported languages.");
listLanguagesCommand.SetHandler(() =>
{
	foreach (var language in Languages.GetSupportedLanguages())
	{
		Console.WriteLine($"{language.TwoLetterISOCode,-6}\t{language.Name}");
	}
});
listCommand.AddCommand(listLanguagesCommand);

// list encodings
var listEncodingsCommand = new Command("encodings", "List supported encodings.");
listEncodingsCommand.SetHandler(() =>
{
	foreach (var encoding in Encoding.GetEncodings())
	{
		Console.WriteLine($"{encoding.CodePage,-6}\t{encoding.Name,-16}\t{encoding.DisplayName}");
	}
});
listCommand.AddCommand(listEncodingsCommand);

// encrypt
var encryptCommand = new Command("encrypt", "Encrypt plaintext.");
encryptCommand.AddAlias("e");
encryptCommand.AddArgument(cipherArgument);
encryptCommand.AddOption(keyArgument);
encryptCommand.AddArgument(textArgument);
encryptCommand.AddOption(simpleDetailedOption);
encryptCommand.SetHandler((BindingContext context) =>
{
	var detailed = context.GetShowDetails(simpleDetailedOption);
	if (!detailed) logger = NullLogger.Instance;
	var text = context.GetText(textArgument);
	var hasKey = context.ParseResult.GetValueForOption(keyArgument) is not null;

	string result;
	if (hasKey)
	{
		var cipher = context.GetKeyedCipher(cipherArgument);
		logger.LogInformation("Cipher: {cipher}", cipher.GetType().Name);
		var keyType = cipher.GetType().GetInterface("IKeyedCipher`1")!.GenericTypeArguments[0];
		var key = context.GetKey(keyArgument, keyType);
		logger.LogInformation("Key: {key}", key);

		var method = typeof(CipherExtensions).Assembly.ExportedTypes.Single(t => t.Name == nameof(CipherExtensions)).GetMethods(BindingFlags.Public | BindingFlags.Static)
			.Where(m => m.Name == nameof(CipherExtensions.Encrypt))
			.Where(m => m.IsGenericMethodDefinition)
			.Where(m => m.GetParameters().Length == 3)
			.Where(m => m.GetParameters()[1].ParameterType == typeof(string))
			.SingleOrDefault()!
			.MakeGenericMethod(keyType);
		result = (string)method.Invoke(null, new[] { cipher, text, key })!;
	}
	else
	{
		var cipher = context.GetCipher(cipherArgument);
		logger.LogInformation("Cipher: {cipher}", cipher.GetType().Name);
		result = cipher.Encrypt(text);
	}

	logger.LogInformation("Ciphertext: {ciphertext}", result);
	logger.LogInformation("HEX: {hex}", Convert.ToHexString(Encoding.UTF8.GetBytes(result)));

	if (!detailed)
	{
		Console.WriteLine(result);
	}
});
rootCommand.AddCommand(encryptCommand);

// decrypt
var decryptCommand = new Command("decrypt", "Decrypt ciphertext.");
decryptCommand.AddAlias("d");
decryptCommand.AddArgument(cipherArgument);
decryptCommand.AddOption(keyArgument);
decryptCommand.AddArgument(textArgument);
decryptCommand.AddOption(simpleDetailedOption);
decryptCommand.SetHandler((BindingContext context) =>
{
	var detailed = context.GetShowDetails(simpleDetailedOption);
	if (!detailed) logger = NullLogger.Instance;
	var text = context.GetText(textArgument);
	var hasKey = context.ParseResult.GetValueForOption(keyArgument) is not null;

	string result;
	if (hasKey)
	{
		var cipher = context.GetKeyedCipher(cipherArgument);
		logger.LogInformation("Cipher: {cipher}", cipher.GetType().Name);
		var keyType = cipher.GetType().GetInterface("IKeyedCipher`1")!.GenericTypeArguments[0];
		var key = context.GetKey(keyArgument, keyType);
		logger.LogInformation("Key: {key}", key);

		var method = typeof(CipherExtensions).Assembly.ExportedTypes.Single(t => t.Name == nameof(CipherExtensions)).GetMethods(BindingFlags.Public | BindingFlags.Static)
			.Where(m => m.Name == nameof(CipherExtensions.Decrypt))
			.Where(m => m.IsGenericMethodDefinition)
			.Where(m => m.GetParameters().Length == 3)
			.Where(m => m.GetParameters()[1].ParameterType == typeof(string))
			.SingleOrDefault()!
			.MakeGenericMethod(keyType);
		result = (string)method.Invoke(null, new[] { cipher, text, key })!;
	}
	else
	{
		var cipher = context.GetCipher(cipherArgument);
		logger.LogInformation("Cipher: {cipher}", cipher.GetType().Name);
		result = cipher.Decrypt(text);
	}

	logger.LogInformation("Plaintext: {ciphertext}", result);
	logger.LogInformation("HEX: {hex}", Convert.ToHexString(Encoding.UTF8.GetBytes(result)));

	if (!detailed)
	{
		Console.WriteLine(result);
	}
});
rootCommand.AddCommand(decryptCommand);

// findkey
var wordlistOption = new Option<string?>("--wordlist", "Specify path of wordlist file.");
wordlistOption.AddAlias("-w");
var scorerOption = new Option<string>("--scorer", "Specify the scorer for scoring speculative plaintexts.");
scorerOption.AddAlias("-s");
scorerOption.SetDefaultValue("letters");
scorerOption.AddCompletions(
	"letters",
	"first-letters",
	"n-grams-2",
	"n-grams-3"
);
var substringOption = new Option<string[]>("--substring", "Specify a search text for scoring speculative plaintexts.") { Arity = ArgumentArity.ZeroOrMore };
var regexOption = new Option<string?>("--regex", "Specify a regular expression for scoring speculative plaintexts.");
var wordlistScorerOption = new Option<string?>("--substring-wordlist", "Specify a file path for a wordlist for scoring speculatve plaintexts.");
var allOverThresholdOption = new Option<double?>("--all-over-threshold", "Show all intermediate candidates above a specified threshold.");
var bestOption = new Option<bool>("--best", "Show only progressively better candidates.");
bestOption.SetDefaultValue(true);
var findKeyCommand = new Command("findkey", "Find a key for a cipher.")
{
	languageOption,
	alphabetOption,
	encodingOption,

	wordlistOption,

	scorerOption,
	substringOption,
	regexOption,
	wordlistScorerOption,

	bestOption,
	allOverThresholdOption,

	analysisDetailedOption
};
findKeyCommand.AddArgument(textArgument);
findKeyCommand.SetHandler(async (
	string ciphertext,
	BindingContext bindingContext,
	CancellationToken cancellationToken
) =>
{
	var detailed = bindingContext.GetShowDetails(analysisDetailedOption);
	if (!detailed) logger = NullLogger.Instance;

	// configure
	var language = bindingContext.GetLanguage(languageOption);
	logger.LogInformation("Language: {language}", language);

	var alphabet = bindingContext.GetAlphabet(alphabetOption, language);
	logger.LogInformation("Alphabet: {alphabet}", alphabet.ToString());

	var encoding = bindingContext.GetEncoding(encodingOption);
	logger.LogInformation("Encoding: {encoding}", encoding.GetType().Name);

	var scorer = bindingContext.GetScorer(substringOption, wordlistScorerOption, regexOption, scorerOption, language, encoding);
	logger.LogInformation("Scorer: {scorer}", scorer.GetType().Name);

	var candidatePromoter = bindingContext.GetPromoter(bestOption, allOverThresholdOption);
	logger.LogInformation("Promoter: {promoter}", candidatePromoter.GetType().Name);

	// build and start
	var cipher = bindingContext.GetKeyedCipher(cipherArgument);
	logger.LogInformation("Cipher: {cipher}", cipher.GetType().Name);
	var keyType = cipher.GetType().GetInterface("IKeyedCipher`1")!.GenericTypeArguments[0];

	logger.LogInformation("Starting...");
	var watch = Stopwatch.StartNew();

	SolverResult? bestResult = null;
	var builder = new StringBuilder();
	var formatArgs = new List<object?>();/*
	await foreach (var candidate in solver.SolveAsync(ciphertext, candidatePromoter, cancellationToken))
	{
		if (bestResult == null || candidate.SpeculativePlaintext.Score > bestResult.SpeculativePlaintext.Score)
		{
			bestResult = candidate;
		}

		builder.AppendLine(new String('>', 32));
		builder.Append("Candidate found with cipher '{cipher}' and score '{score}'.");
		formatArgs.Add(candidate.Cipher.Cipher.GetType().Name);
		formatArgs.Add(candidate.SpeculativePlaintext.Score);
		builder.AppendLine();

		if (candidate.Cipher.Cipher.GetType().GetProperty("Options") is PropertyInfo optionsProperty)
		{
			builder.Append("Options: {options}");
			formatArgs.Add(optionsProperty.GetValue(candidate.Cipher.Cipher));
			builder.AppendLine();
		}
		if (candidate.Cipher.HasKey)
		{
			builder.Append("Key: {key}");
			formatArgs.Add(candidate.Cipher.Key);
			builder.AppendLine();
		}
		builder.Append("Plaintext: {plaintext}");
		formatArgs.Add(candidate.SpeculativePlaintext.Plaintext);
		builder.AppendLine();
		builder.Append("HEX (utf8): {hex}");
		formatArgs.Add(Convert.ToHexString(Encoding.UTF8.GetBytes(candidate.SpeculativePlaintext.Plaintext)));
		builder.AppendLine();

		logger.LogInformation(builder.ToString(), args: formatArgs.ToArray());
		builder.Clear();
		formatArgs.Clear();
	}*/

	// finish
	watch.Stop();
	logger.LogInformation("Finished.");
	logger.LogInformation("Elapsed: {elapsed}", watch.Elapsed);

	if (!detailed && bestResult != null)
	{
		Console.WriteLine(bestResult.SpeculativePlaintext.Plaintext);
	}
}, textArgument);
rootCommand.AddCommand(findKeyCommand);

// solve
var solveCommand = new Command("solve", "Solve a cryptogram.")
{
	languageOption,
	alphabetOption,
	encodingOption,

	wordlistOption,

	scorerOption,
	substringOption,
	regexOption,
	wordlistScorerOption,

	bestOption,
	allOverThresholdOption,

	analysisDetailedOption
};
solveCommand.AddArgument(textArgument);
solveCommand.SetHandler(async (
	string ciphertext,
	BindingContext bindingContext,
	CancellationToken cancellationToken
) =>
{
	var detailed = bindingContext.GetShowDetails(analysisDetailedOption);
	if (!detailed) logger = NullLogger.Instance;

	// configure
	var language = bindingContext.GetLanguage(languageOption);
	logger.LogInformation("Language: {language}", language);

	var alphabet = bindingContext.GetAlphabet(alphabetOption, language);
	logger.LogInformation("Alphabet: {alphabet}", alphabet.ToString());

	var encoding = bindingContext.GetEncoding(encodingOption);
	logger.LogInformation("Encoding: {encoding}", encoding.GetType().Name);

	var scorer = bindingContext.GetScorer(substringOption, wordlistScorerOption, regexOption, scorerOption, language, encoding);
	logger.LogInformation("Scorer: {scorer}", scorer.GetType().Name);

	var candidatePromoter = bindingContext.GetPromoter(bestOption, allOverThresholdOption);
	logger.LogInformation("Promoter: {promoter}", candidatePromoter.GetType().Name);

	// build and start
	var solver = new CryptogramSolverBuilder(scorer)
		.AddCipher(new ReverseCipher())
		.AddCipher(new AtbashCipher(alphabet))

		.AddCipher(new AsciiBinaryBase(BinaryOptions.Default))
		.AddCipher(new HexEncoder(encoding))
		.AddCipher(new Base64Encoder(encoding))
		.AddCipher(new Base64Encoder(encoding, Base64Options.Url))
		.AddCipher(new Base64Encoder(encoding, Base64Options.Imap))

		.AddCipher(new InternationalMorseCode())
		.AddCipher(new InternationalMorseCode(new(Dot: '-', Dash: '.', Delimiter: ' '))) // reverse
		.AddCipher(new TapCode())
		.AddCipher(new BaconCipher())
		.AddCipher(new BaconCipher(new(A: 'B', B: 'A'))) // reverse
		.AddCipher(new WolfenbüttelerCipher())
		.AddCipher(new MalespinSlang())

		.AddCipher(new ScytaleCipher(), new IntKeySpace(minimum: 2, maximum: ciphertext.Length / 2))
		.AddCipher(new CaesarCipher(alphabet))
		.AddCipher(new Rot13Cipher(alphabet))
		.AddCipher(new Rot47Cipher())
		.AddCipher(new ShiftCipher(alphabet), new ShiftKeySpace(alphabet))
		.AddCipher(new AffineCipher(alphabet), new AffineKeySpace(alphabet))
		.AddCipher(new TrithemiusCipher(alphabet))

		.AddCipher(new NthCharacterNullCipher(), new NthCharacterKeySpace(10, 10))
		.AddCipher(new NthLetterNullCipher(), new NthCharacterKeySpace(10, 10))
		.AddCipher(new NthLetterOfWordsNullCipher(), new IntKeySpace(0, 10))

		.AddCipher(new XorCipher(), new ArrayKeySpace<int>(1, 3, Enumerable.Range(32, 96).ToHashSet()))
		.SetLogger(logger)
		.Build();

	logger.LogInformation("Starting...");
	var watch = Stopwatch.StartNew();

	SolverResult? bestResult = null;
	var builder = new StringBuilder();
	var formatArgs = new List<object?>();
	await foreach (var candidate in solver.SolveAsync(ciphertext, candidatePromoter, cancellationToken))
	{
		if (bestResult == null || candidate.SpeculativePlaintext.Score > bestResult.SpeculativePlaintext.Score)
		{
			bestResult = candidate;
		}

		builder.AppendLine(new String('>', 32));
		builder.Append("Candidate found with cipher '{cipher}' and score '{score}'.");
		formatArgs.Add(candidate.Cipher.Cipher.GetType().Name);
		formatArgs.Add(candidate.SpeculativePlaintext.Score);
		builder.AppendLine();

		if (candidate.Cipher.Cipher.GetType().GetProperty("Options") is PropertyInfo optionsProperty)
		{
			builder.Append("Options: {options}");
			formatArgs.Add(optionsProperty.GetValue(candidate.Cipher.Cipher));
			builder.AppendLine();
		}
		if (candidate.Cipher.HasKey)
		{
			builder.Append("Key: {key}");
			formatArgs.Add(candidate.Cipher.Key);
			builder.AppendLine();
		}
		builder.Append("Plaintext: {plaintext}");
		formatArgs.Add(candidate.SpeculativePlaintext.Plaintext);
		builder.AppendLine();
		builder.Append("HEX (utf8): {hex}");
		formatArgs.Add(Convert.ToHexString(Encoding.UTF8.GetBytes(candidate.SpeculativePlaintext.Plaintext)));
		builder.AppendLine();

		logger.LogInformation(builder.ToString(), args: formatArgs.ToArray());
		builder.Clear();
		formatArgs.Clear();
	}

	// finish
	watch.Stop();
	logger.LogInformation("Finished.");
	logger.LogInformation("Elapsed: {elapsed}", watch.Elapsed);

	if (!detailed && bestResult != null)
	{
		Console.WriteLine(bestResult.SpeculativePlaintext.Plaintext);
	}
}, textArgument);
rootCommand.AddCommand(solveCommand);

// frequency analysis
var frequencyAnalysisCommand = new Command("frequency-analysis", "Frequency analysis");
frequencyAnalysisCommand.AddAlias("fa");
frequencyAnalysisCommand.AddArgument(textArgument);
frequencyAnalysisCommand.SetHandler((string text, BindingContext context) =>
{
	var result = FrequencyAnalysis.Analyze(text, _ => true, IgnoreCaseCharComparer.Instance);
	var sum = (double)result.Sum(r => r.Value);
	if (sum <= 0D)
	{
		return;
	}
	foreach (var item in result.OrderBy(k => k.Key))
	{
		Console.WriteLine($"{item.Key,-2}\t{item.Value,-4}\t{item.Value / sum:N4}");
	}
}, textArgument);
rootCommand.AddCommand(frequencyAnalysisCommand);

// caesar bruteforce
var caesarBruteforceCommand = new Command("caesar-bruteforce", "Caesar bruteforce");
caesarBruteforceCommand.AddAlias("cbf");
caesarBruteforceCommand.AddArgument(textArgument);
caesarBruteforceCommand.AddOption(languageOption);
caesarBruteforceCommand.AddOption(alphabetOption);
caesarBruteforceCommand.SetHandler((string text, BindingContext bindingContext) =>
{
	var language = bindingContext.GetLanguage(languageOption);
	logger.LogInformation("Language: {language}", language);

	var alphabet = bindingContext.GetAlphabet(alphabetOption, language);
	logger.LogInformation("Alphabet: {alphabet}", alphabet.ToString());

	var result = CaesarBruteforce.Analyze(text, alphabet);
	foreach (var item in result.OrderBy(k => k.Key))
	{
		Console.WriteLine($"{item.Key,-2}\t{item.Value}");
	}
}, textArgument);
rootCommand.AddCommand(caesarBruteforceCommand);

// n-gram analysis
var ngramAnalysisCommand = new Command("ngram-analysis", "NGram analysis");
ngramAnalysisCommand.AddAlias("nga");
ngramAnalysisCommand.AddArgument(textArgument);
var ngramLengthOption = new Option<int>("--length", "Length of n-grams");
ngramLengthOption.SetDefaultValue(2);
ngramAnalysisCommand.AddOption(ngramLengthOption);
ngramAnalysisCommand.SetHandler((string text, int length, BindingContext context) =>
{
	var result = NGramAnalysis.Analyze(text, length, StringComparer.OrdinalIgnoreCase);
	var sum = (double)result.Sum(r => r.Value);
	if (sum <= 0D)
	{
		return;
	}
	foreach (var item in result.OrderBy(k => k.Key))
	{
		Console.WriteLine($"{item.Key,-4}\t{item.Value,-4}\t{item.Value / sum:N4}");
	}
}, textArgument, ngramLengthOption);
rootCommand.AddCommand(ngramAnalysisCommand);

// score
var scoreCommand = new Command("score", "Score a speculative plaintext.")
{
	languageOption,
	alphabetOption,

	scorerOption,
	substringOption,
	regexOption,
	wordlistScorerOption,

	analysisDetailedOption,
};
scoreCommand.AddArgument(textArgument);
scoreCommand.SetHandler((string plaintext, BindingContext bindingContext) =>
{
	var detailed = bindingContext.GetShowDetails(analysisDetailedOption);
	if (!detailed) logger = NullLogger.Instance;

	var language = bindingContext.GetLanguage(languageOption);
	logger.LogInformation("Language: {language}", language);

	var alphabet = bindingContext.GetAlphabet(alphabetOption, language);
	logger.LogInformation("Alphabet: {language}", alphabet.ToString());

	var encoding = bindingContext.GetEncoding(encodingOption);
	logger.LogInformation("Encoding: {encoding}", encoding.GetType().Name);

	var scorer = bindingContext.GetScorer(substringOption, wordlistScorerOption, regexOption, scorerOption, language, encoding);
	logger.LogInformation("Scorer: {scorer}", scorer.GetType().Name);

	var score = scorer.Score(plaintext);
	logger.LogInformation("Score: {score}", score);

	if (!detailed)
	{
		Console.WriteLine(score.ToString(CultureInfo.InvariantCulture));
	}
}, textArgument);
rootCommand.Add(scoreCommand);

// run
await rootCommand.InvokeAsync(args);
Console.ResetColor();
