using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using RomanNumerals.Numerals;
using RomanNumerals.Utility;

namespace RomanNumerals;

[DebuggerDisplay("{Value} --> {Literal}")]
public class Numeral
{
    public string Literal { get; }
    public uint Value { get; }
    public NumeralFlags Flags { get; }
    public NumeralFlags ExcludeFlags { get; }

    public Numeral(string literal, uint value, NumeralFlags flags = NumeralFlags.Ascii, NumeralFlags excludeFlags = 0)
    {
        Literal = literal;
        Value = value;
        Flags = flags;
        ExcludeFlags = excludeFlags;
    }
}

internal static class NumeralExtensions
{
    public static IEnumerable<Numeral> WithVinculum(this Numeral numeral)
    {
        yield return numeral;
        yield return CreateVinculum(numeral, '\u0305', 1_000);
        yield return CreateVinculum(numeral, '\u033F', 1_000_000);
    }

    public static IEnumerable<Numeral> Single(this Numeral numeral)
    {
        yield return numeral;
    }

    private static Numeral CreateVinculum(this Numeral numeral, char marker, int factor)
    {
        var literal = new StringBuilder();
        foreach (var c in numeral.Literal)
            literal.AppendFormat("{0}{1}", c, marker);
        return new Numeral(literal.ToString(), (uint)(numeral.Value * factor), numeral.Flags | NumeralFlags.Vinculum);
    }
}

public class NumeralsSet
{
    public uint Base { get; }

    private readonly ICollection<Numeral> _numerals;
    private readonly IDictionary<uint, ICollection<Numeral>> _numeralsByValue;
    private readonly Dictionary<string, string> _ligatures;

    public static readonly NumeralsSet Default = new NumeralsSet(10, CreateDefaultNumerals(), new Dictionary<string, string>
    {
        {"I","Ⅰ"},
        {"II","Ⅱ"},
        {"III","Ⅲ"},
        {"IV","Ⅳ"},
        {"V","Ⅴ"},
        {"VI","Ⅵ"},
        {"VII","Ⅶ"},
        {"VIII","Ⅷ"},
        {"IX","Ⅸ"},
        {"X","Ⅹ"},
        {"XI","Ⅺ"},
        {"XII","Ⅻ"},
        {"L","Ⅼ"},
        {"C","Ⅽ"},
        {"D","Ⅾ"},
        {"M","Ⅿ"},
        {"(|)","ↀ"},
        {"|))","ↁ"},
        {"((|))","ↂ"},
        {"|)))","ↇ"},
        {"(((|)))"," ↈ"},
    });

    private static IEnumerable<Numeral> CreateDefaultNumerals()
    {
        return new Numeral[]
        {
            new("I", 1),
            new("V", 5),
            new("X", 10),
            new("L", 50),
            new("C", 100),
            new("D", 500),
            new("M", 1000),
        }.SelectMany(n => n.WithVinculum());
    }

    public NumeralsSet(uint @base, IEnumerable<Numeral> numerals, Dictionary<string, string> ligatures)
    {
        Base = @base;
        _numerals = numerals.ToArray();
        _numeralsByValue = _numerals.GroupBy(n => n.Value).ToDictionary(n => n.Key, n => (ICollection<Numeral>)n.ToArray());
        _ligatures = ligatures;
    }

    internal NumeralTriplet GetTriplet(uint pow)
    {
        var nextPow = pow * Base;
        return new NumeralTriplet(_numeralsByValue.TryGetOrDefault(pow)?.FirstOrDefault(),
            _numeralsByValue.TryGetOrDefault(nextPow / 2)?.FirstOrDefault(),
            _numeralsByValue.TryGetOrDefault(nextPow)?.FirstOrDefault());
    }
}

public class NumeralTriplet
{
    public Numeral Unit { get; }

    public Numeral HalfUpperUnit { get; }

    public Numeral UpperUnit { get; }

    public NumeralTriplet(Numeral unit, Numeral halfUpperUnit, Numeral upperUnit)
    {
        Unit = unit;
        HalfUpperUnit = halfUpperUnit;
        UpperUnit = upperUnit;
    }
}

public class NumeralBuilderOptions
{
    public int SubtractableDigits { get; set; } = 1;

    public NumeralFlags Flags { get; set; } = NumeralFlags.Ascii;

    public static NumeralBuilderOptions Default => new NumeralBuilderOptions();
}

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

    public string ToString(uint value)
    {
        var literal = new StringBuilder();
        foreach (var (digit, pow) in GetDigits(value).Reverse())
            Append(digit, pow, literal);
        return literal.ToString();
    }

    private IEnumerable<DigitAndPow> GetDigits(uint value)
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

    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        throw new NotImplementedException();
    }

    private void Append(uint value, uint pow, StringBuilder literal)
    {
        if (value == 0)
            return;
        var triplet = _numeralsSet.GetTriplet(pow);
        if (triplet.Unit is null)
            throw new OverflowException($"No unit available for {value}") { Data = { { "Value", value } } };
        foreach (var numeral in GetNumerals(value, triplet))
            literal.Append(numeral.Literal);
    }

    private IEnumerable<Numeral> GetNumerals(uint value, NumeralTriplet numeralTriplet)
    {
        // [I*X..X[
        var subtractableValue = _options.SubtractableDigits * numeralTriplet.Unit.Value;
        if (value >= numeralTriplet.UpperUnit?.Value - subtractableValue)
        {
            for (var count = value; count < numeralTriplet.UpperUnit.Value; count += numeralTriplet.Unit.Value)
                yield return numeralTriplet.Unit;
            yield return numeralTriplet.UpperUnit;
            yield break;
        }
        // [V..VIII*]
        if (value >= numeralTriplet.HalfUpperUnit?.Value)
        {
            yield return numeralTriplet.HalfUpperUnit;
            for (var count = numeralTriplet.HalfUpperUnit.Value; count < value; count += numeralTriplet.Unit.Value)
                yield return numeralTriplet.Unit;
            yield break;
        }
        // [I*V..V[
        if (value >= numeralTriplet.HalfUpperUnit?.Value - subtractableValue)
        {
            for (var count = value; count < numeralTriplet.HalfUpperUnit.Value; count += numeralTriplet.Unit.Value)
                yield return numeralTriplet.Unit;
            yield return numeralTriplet.HalfUpperUnit;
            yield break;
        }
        // [I..III*]
        for (var count = 0u; count < value; count += numeralTriplet.Unit.Value)
            yield return numeralTriplet.Unit;
    }
}
