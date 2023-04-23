using System;
using RomanNumerals.Numerals;

namespace RomanNumerals;

/// <summary>
///     Conversion class
/// </summary>
public static class Convert
{
    /// <summary>
    ///     Converts the given <see cref="uint" /> to Roman numerals
    /// </summary>
    /// <param name="value"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static string ToRomanNumerals(this uint value, NumeralFlags flags = 0)
    {
        return Numerals.NumeralBuilder.Build(value, flags);
    }

    /// <summary>
    ///     Converts the given <see cref="int" /> to Roman numerals
    /// </summary>
    /// <param name="value"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static string ToRomanNumerals(this int value, NumeralFlags flags = 0)
    {
        if (value < 0)
            throw new ArgumentException("Only positive integers are supported");
        return Numerals.NumeralBuilder.Build((uint) value, flags);
    }

    public static uint? TryFromRomanNumerals(this string s)
    {
        throw new NotImplementedException();
    }
}