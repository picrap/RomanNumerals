namespace RomanNumerals.Numerals
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class NumeralBuilder
    {
        public static string Build(uint v, NumeralFlags flags = 0)
        {
            var builder = new StringBuilder();
            foreach (var digit in EnumerateDigits(v))
            {
                if (digit == 0)
                    continue;

                var literalNumeral = RomanNumeralsDefinition.TryGet(digit,flags);
                if (literalNumeral is null)
                    throw new ArgumentException("Can't convert number");
                builder.Append(literalNumeral.Literal);
            }

            return builder.ToString();
        }

        private static IEnumerable<uint> EnumerateDigits(uint v)
        {
            var log10 = Math.Floor(Math.Log10(v));
            var m = (int)Math.Pow(10, log10);
            for (var l = log10; l >= 0; l--)
            {
                var digit = (v / m) % 10 * m;
                yield return (uint)digit;
                m /= 10;
            }
        }
    }
}