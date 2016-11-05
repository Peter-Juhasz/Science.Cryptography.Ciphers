namespace Science.Cryptography.Ciphers
{
    public class MorseCodeConfiguration
    {
        public char Dot { get; set; }

        public char Dash { get; set; }

        public char Separator { get; set; }


        public static readonly MorseCodeConfiguration Default = new MorseCodeConfiguration
        {
            Dot = '.',
            Dash = '-',
            Separator = ' ',
        };
    }
}
