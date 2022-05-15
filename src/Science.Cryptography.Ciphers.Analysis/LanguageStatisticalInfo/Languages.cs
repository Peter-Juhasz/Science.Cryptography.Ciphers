using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Science.Cryptography.Ciphers.Analysis;

/// <summary>
/// Contains reference data for languages.
/// </summary>
public static partial class Languages
{
	public static LanguageStatisticalInfo FromTwoLetterISOName(string name) => _map[name];

	public static LanguageStatisticalInfo FromCultureInfo(CultureInfo cultureInfo) => FromTwoLetterISOName(cultureInfo.TwoLetterISOLanguageName);

	private static readonly Dictionary<string, LanguageStatisticalInfo> _map = new();

	public static IReadOnlySet<LanguageStatisticalInfo> GetSupportedLanguages() => _map.Values.ToHashSet();

	static Languages()
	{
		_map.Add("en", English);
	}
}
