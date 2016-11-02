using System.Collections.Generic;
using System.Globalization;

namespace Science.Cryptography.Ciphers.Analysis
{
    /// <summary>
    /// Contains reference data for languages.
    /// </summary>
    public static partial class Languages
    {
        public static ILanguageStatisticalInfo FromCultureInfo(CultureInfo cultureInfo)
        {
            return CultureInfoMap[cultureInfo];
        }

        private static IReadOnlyDictionary<CultureInfo, ILanguageStatisticalInfo> CultureInfoMap = new Dictionary<CultureInfo, ILanguageStatisticalInfo>
        {
            { new CultureInfo("en-us"), English },
        };
    }
}
