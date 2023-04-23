using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomanNumerals.Numerals;

public class NumeralBuilder : ICustomFormatter
{
    private struct DigitAndPow
    {
        public uint Digit { get; }
        public uint Pow { get; }

        public DigitAndPow(uint digit, uint pow)
        {
            Digit = digit;
            Pow = pow;
        }

        public void Deconstruct(out uint digit, out uint pow)
        {
            digit = Digit;
            pow = Pow;
        }
    }

    private readonly NumeralsSet _numeralsSet;
    private readonly NumeralBuilderOptions _options;

    public NumeralBuilder(NumeralsSet numeralsSet = null, NumeralBuilderOptions options = null)
    {
        _numeralsSet = numeralsSet ?? NumeralsSet.Default;
        _options = options ?? NumeralBuilderOptions.Default;
    }

    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        throw new NotImplementedException();
    }

    public string ToString(uint value) => ToString(value, _options, _numeralsSet);

    private static string ToString(uint value, NumeralBuilderOptions options, NumeralsSet numeralsSet)
    {
        var parts = GetLiteralNumerals(value, options, numeralsSet).ToList();
        while (parts.Count >= 2)
        {
            var lastPart = parts[parts.Count - 2] + parts[parts.Count - 1];
            var escapedLastPart = CheckLigatures(lastPart, options, numeralsSet);
            if (escapedLastPart == lastPart)
                break;
            parts.RemoveRange(parts.Count - 2, 2);
            parts.Add(escapedLastPart);
        }
        return string.Join("", parts);
    }

    private static IEnumerable<string> GetLiteralNumerals(uint value, NumeralBuilderOptions numeralBuilderOptions, NumeralsSet numeralsSet)
    {
        foreach (var (digit, pow) in GetDigitsAndPows(value, numeralsSet).Reverse())
        {
            var part = GetLiteralNumeral(digit, pow, numeralBuilderOptions, numeralsSet);
            part = CheckLigatures(part, numeralBuilderOptions, numeralsSet);
            yield return part;
        }
    }

    private static string CheckUnicode(string value, NumeralBuilderOptions options, NumeralsSet numeralsSet)
    {
        if (options.Style.HasFlag(NumeralBuilderStyle.Unicode))
            return numeralsSet.GetUnicodeAlias(value);
        return value;
    }

    private static string CheckLigatures(string value, NumeralBuilderOptions options, NumeralsSet numeralsSet)
    {
        if (options.Style.HasFlag(NumeralBuilderStyle.Ligature))
            return numeralsSet.GetUnicodeLigature(value);
        return value;
    }

    private static IEnumerable<DigitAndPow> GetDigitsAndPows(uint value, NumeralsSet numeralsSet)
    {
        for (var pow = 1u; value != 0; pow *= numeralsSet.Base)
        {
            var nextPow = pow * numeralsSet.Base;
            var digit = value % nextPow;
            if (digit != 0)
                yield return new(digit, pow);
            value -= digit;
        }
    }

    private static string GetLiteralNumeral(uint digit, uint pow, NumeralBuilderOptions options, NumeralsSet numeralsSet)
    {
        if (digit == 0)
            return "";
        var triplet = numeralsSet.GetTriplet(pow, options.Kind);
        if (triplet.Unit is null)
            throw new OverflowException($"No numeral available for {pow}") { Data = { { "Value", digit }, { "Pow", pow } } };
        var literal = new StringBuilder();
        foreach (var numeral in GetDigitNumerals(digit, triplet, options))
        {
            var numeralLiteral = numeral.Literal;
            numeralLiteral = CheckUnicode(numeralLiteral, options, numeralsSet);
            literal.Append(numeralLiteral);
        }

        return literal.ToString();
    }

    private static IEnumerable<Numeral> GetDigitNumerals(uint digit, NumeralTriplet numeralTriplet, NumeralBuilderOptions options)
    {
        // [I*X..X[
        var subtractableValue = options.SubtractableDigits * numeralTriplet.Unit.Value;
        if (digit >= numeralTriplet.UpperUnit?.Value - subtractableValue && !numeralTriplet.UpperUnit.Options.HasFlag(NumeralOptions.NoSubtract))
        {
            for (var count = digit; count < numeralTriplet.UpperUnit.Value; count += numeralTriplet.Unit.Value)
                yield return numeralTriplet.Unit;
            yield return numeralTriplet.UpperUnit;
            yield break;
        }
        // [V..VIII*]
        if (digit >= numeralTriplet.HalfUpperUnit?.Value)
        {
            yield return numeralTriplet.HalfUpperUnit;
            for (var count = numeralTriplet.HalfUpperUnit.Value; count < digit; count += numeralTriplet.Unit.Value)
                yield return numeralTriplet.Unit;
            yield break;
        }
        // [I*V..V[
        if (digit >= numeralTriplet.HalfUpperUnit?.Value - subtractableValue && !numeralTriplet.HalfUpperUnit.Options.HasFlag(NumeralOptions.NoSubtract))
        {
            for (var count = digit; count < numeralTriplet.HalfUpperUnit.Value; count += numeralTriplet.Unit.Value)
                yield return numeralTriplet.Unit;
            yield return numeralTriplet.HalfUpperUnit;
            yield break;
        }
        // [I..III*]
        for (var count = 0u; count < digit; count += numeralTriplet.Unit.Value)
            yield return numeralTriplet.Unit;
    }
}
