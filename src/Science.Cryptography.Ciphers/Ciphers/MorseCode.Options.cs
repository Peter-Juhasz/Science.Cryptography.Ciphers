using System.Collections.Generic;

namespace Science.Cryptography.Ciphers;

public class MorseCodeOptions
{
    public char Dot { get; set; } = '.';

    public char Dash { get; set; } = '-';

    public char Delimiter { get; set; } = ' ';


    public static readonly MorseCodeOptions Default = new();

    public CharacterToSegmentSubstitutionMap GetSubstitutionMap() => new(new Dictionary<char, string>()
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
    });
}
