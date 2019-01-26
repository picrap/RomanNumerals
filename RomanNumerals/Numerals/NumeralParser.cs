namespace RomanNumerals.Numerals
{
    using System.Linq;

    /// <summary>
    /// Parser for roman numeral literals
    /// </summary>
    public static class NumeralParser
    {
        /// <summary>
        /// Tries to parse numeral.
        /// </summary>
        /// <remarks>No format flag here, everything is tested</remarks>
        /// <param name="literal"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool TryParse(string literal, out uint value)
        {
            value = 0;
            var numeralsByLength = RomanNumeralsDefinition.LiteralNumerals.SelectMany(kv => kv.Value).OrderByDescending(n => n.Literal.Length).ToArray();
            for (int index = 0; index < literal.Length;)
            {
                var maxLength = literal.Length - index;
                foreach (var n in numeralsByLength)
                {
                    if (n.Literal.Length > maxLength)
                        continue;
                    if (literal.Substring(index, n.Literal.Length) == n.Literal)
                    {
                        value += n.Digit;
                        index += n.Literal.Length;
                        goto next; // oh yeah, this is a goto
                    }
                }

                return false;
            next:;
            }

            return true;
        }
    }
}