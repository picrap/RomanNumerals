namespace RomanNumerals.Numerals
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Builds roman numerals from integers
    /// </summary>
    public static class NumeralBuilder
    {
        /// <summary>
        /// Builds roman numeral from integer
        /// </summary>
        /// <param name="v"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static string Build(uint v, NumeralFlags flags = 0)
        {
            // in case nothing was specified
            if ((flags & (NumeralFlags.Ascii | NumeralFlags.Unicode)) == 0)
                flags |= NumeralFlags.Ascii;
            var builder = new StringBuilder();
            var remaining = v;
            foreach (var digit in EnumerateDigits(v))
            {
                if (digit == 0)
                    continue;

                // hint: if the remaining value fits in one digit, use it
                var literalNumeral = RomanNumeralsDefinition.TryGet(remaining, flags);
                if (literalNumeral != null)
                {
                    builder.Append(literalNumeral.Literal);
                    break;
                }

                // look for single digit
                literalNumeral = RomanNumeralsDefinition.TryGet(digit, flags);
                if (literalNumeral is null)
                    throw new ArgumentOutOfRangeException(nameof(v), "Can't convert number");
                builder.Append(literalNumeral.Literal);
                remaining -= v;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Enumerates the digits one by one, starting from higher value
        /// keeping digits at their place:
        /// </summary>
        /// <example>140012 --> {100000, 40000, 0,0, 10,2}</example>
        /// <param name="v"></param>
        /// <returns></returns>
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