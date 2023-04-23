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

    public string ToString(uint value)
    {
        var parts = GetLiteralNumerals(value).ToList();
        while (parts.Count >= 2)
        {
            var lastPart = parts[parts.Count - 2] + parts[parts.Count - 1];
            var escapedLastPart = CheckLigatures(lastPart);
            if (escapedLastPart == lastPart)
                break;
            parts.RemoveRange(parts.Count - 2, 2);
            parts.Add(escapedLastPart);
        }
        return string.Join("", parts);
    }

    private IEnumerable<string> GetLiteralNumerals(uint value)
    {
        foreach (var (digit, pow) in GetDigitsAndPows(value).Reverse())
        {
            var part = GetLiteralNumeral(digit, pow);
            part = CheckLigatures(part);
            yield return part;
        }
    }

    private string CheckUnicode(string value)
    {
        if (_options.Unicode)
            return _numeralsSet.GetUnicodeAlias(value);
        return value;
    }

    private string CheckLigatures(string value)
    {
        if (_options.Ligature)
            return _numeralsSet.GetUnicodeLigature(value);
        return value;
    }

    private IEnumerable<DigitAndPow> GetDigitsAndPows(uint value)
    {
        for (var pow = 1u; value != 0; pow *= _numeralsSet.Base)
        {
            var nextPow = pow * _numeralsSet.Base;
            var digit = value % nextPow;
            if (digit != 0)
                yield return new(digit, pow);
            value -= digit;
        }
    }

    private string GetLiteralNumeral(uint digit, uint pow)
    {
        if (digit == 0)
            return "";
        var triplet = _numeralsSet.GetTriplet(pow, _options.Kind);
        if (triplet.Unit is null)
            throw new OverflowException($"No numeral available for {pow}") { Data = { { "Value", digit }, { "Pow", pow } } };
        var literal = new StringBuilder();
        foreach (var numeral in GetDigitNumerals(digit, triplet))
        {
            var numeralLiteral = numeral.Literal;
            numeralLiteral = CheckUnicode(numeralLiteral);
            literal.Append(numeralLiteral);
        }

        return literal.ToString();
    }

    private IEnumerable<Numeral> GetDigitNumerals(uint digit, NumeralTriplet numeralTriplet)
    {
        // [I*X..X[
        var subtractableValue = _options.SubtractableDigits * numeralTriplet.Unit.Value;
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
