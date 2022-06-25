using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Science.Cryptography.Ciphers.Analysis.Tests;

[TestClass]
public class LanguagesTests
{
	[TestMethod]
	public void GetEnglish()
	{
		Assert.AreEqual(Languages.English, Languages.FromTwoLetterISOName("en"));
	}
}