namespace RomanNumerals.Numerals
{
    /// <summary>
    /// Defines a literal numeral
    /// </summary>
    public class LiteralNumeral
    {
        /// <summary>
        /// The digit to be translated
        /// </summary>
        /// <example>1, 2 but also 10, 20, etc.</example>
        public uint Digit { get; }
        /// <summary>
        /// The literal representation of digit
        /// </summary>
        public string Literal { get; }
        /// <summary>
        /// Flags that identify the category (ASCII/Unicode, vinculum, etc.)
        /// </summary>
        public NumeralFlags Flags { get; }

        /// <summary>
        /// Instance constructor
        /// </summary>
        /// <param name="literal"></param>
        /// <param name="digit"></param>
        /// <param name="flags"></param>
        public LiteralNumeral(string literal, uint digit, NumeralFlags flags = 0)
        {
            Literal = literal;
            Digit = digit;
            Flags = flags;
        }
        
        /// <summary>
        /// Indicates if this instance can be used with requested flags
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Matches(NumeralFlags flags)
        {
            return (Flags & ~flags) == 0;
        }
    }
}
