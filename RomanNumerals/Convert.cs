﻿using System;
using RomanNumerals.Numerals;

namespace RomanNumerals;

/// <summary>
///     Conversion class
/// </summary>
public static class Convert
{
    private static NumeralBuilderOptions CreateNumeralBuilderOptions(NumeralFlags flags)
    {
        var options = new NumeralBuilderOptions();
        if (flags.HasFlag(NumeralFlags.Unicode))
            options.Style |= NumeralBuilderStyle.Unicode | NumeralBuilderStyle.Ligature;

        if (flags.HasFlag(NumeralFlags.Vinculum))
            options.Kind = NumeralKind.Vinculum;
        else if (flags.HasFlag(NumeralFlags.Apostrophus))
            options.Kind = NumeralKind.Apostrophus;
        return options;
    }

    /// <summary>
    ///     Converts the given <see cref="uint" /> to Roman numerals
    /// </summary>
    /// <param name="value"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static string ToRomanNumerals(this uint value, NumeralFlags flags = 0)
    {
        return NumeralBuilder.Default.ToString(value, CreateNumeralBuilderOptions(flags));
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
        return NumeralBuilder.Default.ToString((uint)value, CreateNumeralBuilderOptions(flags));
    }

    public static bool TryParseRomanNumerals(this string s, out uint v)
    {
        return NumeralParser.Default.TryParse(s, out v);
    }

    public static uint? TryFromRomanNumerals(this string s)
    {
        if (TryParseRomanNumerals(s, out var v))
            return v;
        return null;
    }

    public static uint FromRomanNumerals(this string s)
    {
        return s.TryFromRomanNumerals() ?? throw new FormatException($"Can’t parse {s}") { Data = { { "Literal", s } } };
    }
}