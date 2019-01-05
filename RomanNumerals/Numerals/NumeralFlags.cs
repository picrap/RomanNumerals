namespace RomanNumerals.Numerals
{
    using System;

    /// <summary>
    /// Flags for conversion
    /// </summary>
    [Flags]
    public enum NumeralFlags
    {
        /// <summary>
        /// Allows ASCII characters
        /// When no flag (0) is specified, this is the default
        /// </summary>
        Ascii = 0x01,
        /// <summary>
        /// Allows Unicode characters (which are specifically designed for roman numerals)
        /// </summary>
        Unicode = 0x02,
        /// <summary>
        /// Thousands are marked with a bar above, millions with two bars
        /// </summary>
        Vinculum = 0x04,
    }
}
