using System;

namespace RomanNumerals.Numerals;

[Flags]
public enum NumeralOptions
{
    None = 0,
    NoSubtract = 0x0001,
}