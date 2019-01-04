namespace RomanNumerals.Numerals
{
    using System;

    [Flags]
    public enum NumeralFlags
    {
        Ascii = 0x01,
        Unicode = 0x02,
        Vinculum = 0x04,
    }
}
