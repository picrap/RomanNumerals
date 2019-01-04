
namespace RomanNumerals
{
    using Numerals;

    public static class Convert
    {
        public static string ToRomanNumerals(this uint value, NumeralFlags flags = 0)
        {
            return NumeralBuilder.Build(value, flags);
        }
    }
}
