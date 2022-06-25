using Science.Cryptography.Ciphers;
using Science.Cryptography.Ciphers.Analysis;
using Science.Cryptography.Ciphers.Console;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

internal static class BindingContextExtenions
{
	public static ICipher GetCipher(this InvocationContext context, Argument<string> option) =>
		CipherCatalog.CreateCipher(context.ParseResult.GetValueForArgument(option)!);

	public static object GetKeyedCipher(this InvocationContext context, Argument<string> option) =>
		CipherCatalog.CreateKeyedCipher(context.ParseResult.GetValueForArgument(option)!);

	public static string GetText(this InvocationContext context, Argument<string> option) =>
		context.ParseResult.GetValueForArgument(option)!;

	public static LanguageStatisticalInfo GetLanguage(this InvocationContext context, Option<string> option) =>
		Languages.FromTwoLetterISOName(context.ParseResult.GetValueForOption(option) ?? "en");

	public static Alphabet GetAlphabet(this InvocationContext context, Option<string?> option, LanguageStatisticalInfo language) =>
		context.ParseResult.GetValueForOption(option) ??
		language.Alphabet ??
		WellKnownAlphabets.English;

	public static Encoding GetEncoding(this InvocationContext context, Option<string?> option) =>
		Encoding.GetEncoding(context.ParseResult.GetValueForOption(option)!) ??
		Encoding.ASCII;

	public static bool GetShowDetails(this InvocationContext context, Option<bool> option) =>
		context.ParseResult.GetValueForOption(option);

	public static ISpeculativePlaintextScorer GetScorer(
		this InvocationContext context,
		Option<string[]> substringOption, Option<string?> substringWordlistOption, Option<string?> regexOption, Option<string> scorerOption,
		LanguageStatisticalInfo language, Encoding encoding
	)
	{
		var substring = context.ParseResult.GetValueForOption(substringOption);
		var substringWordlist = context.ParseResult.GetValueForOption(substringWordlistOption);
		var regex = context.ParseResult.GetValueForOption(regexOption);
		var scorerName = context.ParseResult.GetValueForOption(scorerOption);

		return
			substring is [string singleSubstring] ? new SubstringSpeculativePlaintextScorer(singleSubstring) :
			substring is [_, ..] ? new AnySubstringSpeculativePlaintextScorer(substring.ToHashSet()) :
			substringWordlist is not null ? new WordlistSpeculativePlaintextScorer(File.ReadAllLines(substringWordlist)) :
			regex is not null ? new RegexSpeculativePlaintextScorer(new System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.Compiled)) :
			scorerName is "letters" && encoding == Encoding.ASCII ? new AsciiRelativeLetterFrequenciesSpeculativePlaintextScorer(language.RelativeFrequenciesOfLetters) :
			scorerName is "letters" ? new RelativeLetterFrequenciesSpeculativePlaintextScorer(language.RelativeFrequenciesOfLetters) :
			scorerName is "first-letters" ? new RelativeFirstLetterOfWordsFrequenciesSpeculativePlaintextScorer(language.RelativeFrequenciesOfFirstLettersOfWords) :
			scorerName is "n-grams-2" && encoding == Encoding.ASCII ? new TwoGramAsciiLettersRelativeFrequenciesSpeculativePlaintextScorer(language.GetNGramFrequencies(2)) :
			scorerName is "n-grams-2" ? new RelativeNGramFrequenciesSpeculativePlaintextScorer(language.GetNGramFrequencies(2), 2) :
			scorerName is "n-grams-3" ? new RelativeNGramFrequenciesSpeculativePlaintextScorer(language.GetNGramFrequencies(3), 3) :
			new RelativeNGramFrequenciesSpeculativePlaintextScorer(language.GetNGramFrequencies(2), 2);
	}

	public static ICandidatePromoter GetPromoter(
		this InvocationContext context,
		Option<bool> betterOption, Option<double?> thresholdOption
	)
	{
		var allOverThreshold = context.ParseResult.GetValueForOption(thresholdOption);
		var better = context.ParseResult.GetValueForOption(betterOption);

		return
			allOverThreshold != null ? new OverThresholdCandidatePromoter(allOverThreshold.Value) :
			better ? new ProgressivelyBetterCandidatePromoter() :
			new ProgressivelyBetterCandidatePromoter();
	}

	public static object GetKey(this InvocationContext context, Option<string?> option, Type type)
	{
		var serialized = context.ParseResult.GetValueForOption(option)!;
		return type switch
		{
			_ when type == typeof(string) => serialized,
			_ when type == typeof(int) => ReadIntFromAnyBase(serialized),
			_ when type == typeof(byte) => Byte.Parse(serialized),
			_ when type == typeof(int[]) => serialized.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(ReadIntFromAnyBase).ToArray(),
			_ when type == typeof(IReadOnlyList<int>) => serialized.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(ReadIntFromAnyBase).ToArray(),
			_ when type == typeof(byte[]) => serialized.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(ReadByteFromAnyBase).ToArray(),
			_ when type == typeof(IReadOnlyList<byte>) => serialized.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(ReadByteFromAnyBase).ToArray(),
			_ when type == typeof(AffineKey) => new AffineKey(ReadIntFromAnyBase(serialized.Split(',', StringSplitOptions.RemoveEmptyEntries)[0]), ReadIntFromAnyBase(serialized.Split(',', StringSplitOptions.RemoveEmptyEntries)[1])),
			_ when type.GetMethod(nameof(Int32.Parse)) is MethodInfo parseMethod => parseMethod.Invoke(null, new[] { serialized })!,
			_ => throw new NotSupportedException($"Type {type} is not supported.")
		};
	}

	private static int ReadIntFromAnyBase(string serialized) => serialized switch
	{
		_ when serialized.StartsWith("0x") => Convert.ToInt32(serialized, fromBase: 16),
		_ => Int32.Parse(serialized)
	};

	private static byte ReadByteFromAnyBase(string serialized) => serialized switch
	{
		_ when serialized.StartsWith("0x") => Convert.ToByte(serialized, fromBase: 16),
		_ => Byte.Parse(serialized)
	};
}
