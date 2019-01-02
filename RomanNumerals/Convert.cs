
namespace RomanNumerals
{
    using Numerals;

    public static class Convert
    {
        public static string ToRomanNumerals(this uint value)
        {
            return NumeralBuilder.Build(value);
        }
    }
}
