namespace RomanNumerals.Numerals
{
    using System;

    public class LiteralNumeral
    {
        public int Digit { get; }
        public string Literal { get; }

        public int Log10 { get; }

        public LiteralNumeral(string literal, int digit)
        {
            Literal = literal;
            Digit = digit;
            Log10 = (int)Math.Floor(Math.Log10(digit));
        }
    }
}