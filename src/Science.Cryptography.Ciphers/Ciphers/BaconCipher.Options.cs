using System.Collections.Generic;

namespace Science.Cryptography.Ciphers;

public record class BaconOptions(
	char A = 'A',
	char B = 'B'
)
{
	public static readonly BaconOptions Default = new();

	public CharacterToSegmentSubstitutionMap GetSubstitutionMap() => new(new Dictionary<char, string>(IgnoreCaseCharComparer.Instance)
	{
		{ 'A', $"{A}{A}{A}{A}{A}" },
		{ 'B', $"{A}{A}{A}{A}{B}" },
		{ 'C', $"{A}{A}{A}{B}{A}" },
		{ 'D', $"{A}{A}{A}{B}{B}" },
		{ 'E', $"{A}{A}{B}{A}{A}" },
		{ 'F', $"{A}{A}{B}{A}{B}" },
		{ 'G', $"{A}{A}{B}{B}{A}" },
		{ 'H', $"{A}{A}{B}{B}{B}" },
		{ 'I', $"{A}{B}{A}{A}{A}" },
		{ 'J', $"{A}{B}{A}{A}{A}" },
		{ 'K', $"{A}{B}{A}{A}{B}" },
		{ 'L', $"{A}{B}{A}{B}{A}" },
		{ 'M', $"{A}{B}{A}{B}{B}" },
		{ 'N', $"{A}{B}{B}{A}{A}" },
		{ 'O', $"{A}{B}{B}{A}{B}" },
		{ 'P', $"{A}{B}{B}{B}{A}" },
		{ 'Q', $"{A}{B}{B}{B}{B}" },
		{ 'R', $"{B}{A}{A}{A}{A}" },
		{ 'S', $"{B}{A}{A}{A}{B}" },
		{ 'T', $"{B}{A}{A}{B}{A}" },
		{ 'U', $"{B}{A}{A}{B}{B}" },
		{ 'V', $"{B}{A}{A}{B}{B}" },
		{ 'W', $"{B}{A}{B}{A}{A}" },
		{ 'X', $"{B}{A}{B}{A}{B}" },
		{ 'Y', $"{B}{A}{B}{B}{A}" },
		{ 'Z', $"{B}{A}{B}{B}{B}" },
	});

	public CharacterToSegmentSubstitutionMap GetVersion2SubstitutionMap() => new(new Dictionary<char, string>(IgnoreCaseCharComparer.Instance)
	{
		{ 'A', $"{A}{A}{A}{A}{A}" },
		{ 'B', $"{A}{A}{A}{A}{B}" },
		{ 'C', $"{A}{A}{A}{B}{A}" },
		{ 'D', $"{A}{A}{A}{B}{B}" },
		{ 'E', $"{A}{A}{B}{A}{A}" },
		{ 'F', $"{A}{A}{B}{A}{B}" },
		{ 'G', $"{A}{A}{B}{B}{A}" },
		{ 'H', $"{A}{A}{B}{B}{B}" },
		{ 'I', $"{A}{B}{A}{A}{A}" },
		{ 'J', $"{A}{B}{A}{A}{B}" },
		{ 'K', $"{A}{B}{A}{B}{A}" },
		{ 'L', $"{A}{B}{A}{B}{B}" },
		{ 'M', $"{A}{B}{B}{A}{A}" },
		{ 'N', $"{A}{B}{B}{A}{B}" },
		{ 'O', $"{A}{B}{B}{B}{A}" },
		{ 'P', $"{A}{B}{B}{B}{B}" },
		{ 'Q', $"{B}{A}{A}{A}{A}" },
		{ 'R', $"{B}{A}{A}{A}{B}" },
		{ 'S', $"{B}{A}{A}{B}{A}" },
		{ 'T', $"{B}{A}{A}{B}{B}" },
		{ 'U', $"{B}{A}{B}{A}{A}" },
		{ 'V', $"{B}{A}{B}{A}{B}" },
		{ 'W', $"{B}{A}{B}{B}{A}" },
		{ 'X', $"{B}{A}{B}{B}{B}" },
		{ 'Y', $"{B}{B}{A}{A}{A}" },
		{ 'Z', $"{B}{B}{A}{A}{B}" },
	});
}
