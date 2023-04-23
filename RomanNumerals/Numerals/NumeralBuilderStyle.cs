using System;

namespace RomanNumerals.Numerals;

[Flags]
public enum NumeralBuilderStyle
{
    None = 0,
    Unicode = 0x0001,
    Ligature = 0x0002,
}
