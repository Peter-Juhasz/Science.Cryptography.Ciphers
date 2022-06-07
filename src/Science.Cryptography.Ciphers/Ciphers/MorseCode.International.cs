using System.Collections.Generic;
using System.Composition;

namespace Science.Cryptography.Ciphers;

[Export("Morse Code", typeof(ICipher))]
public class InternationalMorseCode : MorseCode
{
	public InternationalMorseCode(MorseCodeOptions options)
		: base(options, options.GetInternationalSubstitutionMap())
	{ }
	public InternationalMorseCode()
		: this(MorseCodeOptions.Default)
	{ }
}

public static class MorseCodeOptionsExtensions
{
	// https://www.itu.int/dms_pubrec/itu-r/rec/m/R-REC-M.1677-1-200910-I!!PDF-E.pdf
	public static CharacterToSegmentSubstitutionMap GetInternationalSubstitutionMap(this MorseCodeOptions options) => options switch
	{
		(char Dot, char Dash, _) => new(new Dictionary<char, string>(IgnoreCaseCharComparer.Instance)
		{
			{ 'A', $"{Dot}{Dash}" },
			{ 'B', $"{Dash}{Dot}{Dot}{Dot}" },
			{ 'C', $"{Dash}{Dot}{Dash}{Dot}" },
			{ 'D', $"{Dash}{Dot}{Dot}" },
			{ 'E', $"{Dot}" },
			{ 'F', $"{Dot}{Dot}{Dash}{Dot}" },
			{ 'G', $"{Dash}{Dash}{Dot}" },
			{ 'H', $"{Dot}{Dot}{Dot}{Dot}" },
			{ 'I', $"{Dot}{Dot}" },
			{ 'J', $"{Dot}{Dash}{Dash}{Dash}" },
			{ 'K', $"{Dash}{Dot}{Dash}" },
			{ 'L', $"{Dot}{Dash}{Dot}{Dot}" },
			{ 'M', $"{Dash}{Dash}" },
			{ 'N', $"{Dash}{Dot}" },
			{ 'O', $"{Dash}{Dash}{Dash}" },
			{ 'P', $"{Dot}{Dash}{Dash}{Dot}" },
			{ 'Q', $"{Dash}{Dash}{Dot}{Dash}" },
			{ 'R', $"{Dot}{Dash}{Dot}" },
			{ 'S', $"{Dot}{Dot}{Dot}" },
			{ 'T', $"{Dash}" },
			{ 'U', $"{Dot}{Dot}{Dash}" },
			{ 'V', $"{Dot}{Dot}{Dot}{Dash}" },
			{ 'W', $"{Dot}{Dash}{Dash}" },
			{ 'X', $"{Dash}{Dot}{Dot}{Dash}" },
			{ 'Y', $"{Dash}{Dot}{Dash}{Dash}" },
			{ 'Z', $"{Dash}{Dash}{Dot}{Dot}" },

			{ '1', $"{Dot}{Dash}{Dash}{Dash}{Dash}" },
			{ '2', $"{Dot}{Dot}{Dash}{Dash}{Dash}" },
			{ '3', $"{Dot}{Dot}{Dot}{Dash}{Dash}" },
			{ '4', $"{Dot}{Dot}{Dot}{Dot}{Dash}" },
			{ '5', $"{Dot}{Dot}{Dot}{Dot}{Dot}" },
			{ '6', $"{Dash}{Dot}{Dot}{Dot}{Dot}" },
			{ '7', $"{Dash}{Dash}{Dot}{Dot}{Dot}" },
			{ '8', $"{Dash}{Dash}{Dash}{Dot}{Dot}" },
			{ '9', $"{Dash}{Dash}{Dash}{Dash}{Dot}" },
			{ '0', $"{Dash}{Dash}{Dash}{Dash}{Dash}" },

			{ '.', $"{Dot}{Dash}{Dot}{Dash}{Dot}{Dash}" },
			{ ',', $"{Dash}{Dash}{Dot}{Dot}{Dash}{Dash}" },
			{ ':', $"{Dash}{Dash}{Dash}{Dot}{Dot}{Dot}" },
			{ '?', $"{Dot}{Dot}{Dash}{Dash}{Dot}{Dot}" },
			{ '\'', $"{Dot}{Dash}{Dash}{Dash}{Dash}{Dot}" },
			{ '-', $"{Dash}{Dot}{Dot}{Dot}{Dot}{Dash}" },
			{ '/', $"{Dash}{Dot}{Dot}{Dash}{Dot}" },
			{ '(', $"{Dash}{Dot}{Dash}{Dash}{Dot}" },
			{ ')', $"{Dash}{Dot}{Dash}{Dash}{Dot}{Dash}" },
			{ '"', $"{Dot}{Dash}{Dot}{Dot}{Dash}{Dot}" },
			{ '=', $"{Dash}{Dot}{Dot}{Dot}{Dash}" },
			{ '+', $"{Dot}{Dash}{Dot}{Dash}{Dot}" },
			{ '×', $"{Dash}{Dot}{Dot}{Dash}" },
			{ '@', $"{Dot}{Dash}{Dash}{Dot}{Dash}{Dot}" },
		})
	};
}
