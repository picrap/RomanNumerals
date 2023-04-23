using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using RomanNumerals.Utility;

namespace RomanNumerals;

public enum NumeralKind
{
    Default = 0,
    Any,
    /// <summary>
    /// Thousands are marked with a bar above, millions with two bars
    /// Vinculum marks are Unicode
    /// </summary>
    Vinculum,
    /// <summary>
    /// Apostrophus extensions: |), (|), |)), ((|)), |))), (((|))) and their Unicode equivalents 
    /// </summary>
    Apostrophus,
}

[Flags]
public enum NumeralOptions
{
    None = 0,
    NoSubtract = 0x0001,
}

[DebuggerDisplay("{Value} --> {Literal}")]
public class Numeral
{
    public string Literal { get; }
    public uint Value { get; }
    public NumeralKind Kind { get; }
    public NumeralOptions Options { get; }


    public Numeral(string literal, uint value, NumeralKind kind = NumeralKind.Default, NumeralOptions options = NumeralOptions.None)
    {
        Literal = literal;
        Value = value;
        Kind = kind;
        Options = options;
    }

    public bool Matches(NumeralKind kind)
    {
        if (Kind == NumeralKind.Any)
            return true;
        return Kind == kind;
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

    public static IDictionary<string, string> WithVinculum(this IDictionary<string, string> aliases)
    {
        return aliases
            .Concat(aliases.Select(kv => CreateVinculum(kv, '\u0305')))
            .Concat(aliases.Select(kv => CreateVinculum(kv, '\u033F')));
    }

    public static IEnumerable<Numeral> Single(this Numeral numeral)
    {
        yield return numeral;
    }

    private static Numeral CreateVinculum(this Numeral numeral, char marker, uint factor)
    {
        return new Numeral(CreateVinculum(numeral.Literal, marker), numeral.Value * factor, NumeralKind.Vinculum);
    }

    private static KeyValuePair<string, string> CreateVinculum(this KeyValuePair<string, string> kv, char marker)
    {
        return new(CreateVinculum(kv.Key, marker), CreateVinculum(kv.Value, marker));
    }

    private static string CreateVinculum(this string s, char marker)
    {
        var v = new StringBuilder();
        foreach (var c in s)
            v.AppendFormat("{0}{1}", c, marker);
        return v.ToString();
    }
}

public class NumeralsSet
{
    public uint Base { get; }

    /// <summary>
    /// Gets the maximum length for a single numeral.
    /// </summary>
    /// <value>
    /// The maximum length.
    /// </value>
    public int MaximumLength { get; }

    private readonly IDictionary<uint, ICollection<Numeral>> _numeralsByValue;
    private IDictionary<string, Numeral> _numeralsByLiteral;
    private IDictionary<string, Numeral> NumeralsByLiteral => _numeralsByLiteral ??= CreateNumeralsByLiteral();

    private readonly IDictionary<string, string> _unicodeAliases;
    private readonly Dictionary<string, string> _ligatures;

    public static readonly NumeralsSet Default = new(10, CreateDefaultNumerals(), CreateUnicodeAliases(), CreateLigatures());

    private static IDictionary<string, string> CreateUnicodeAliases()
    {
        return new Dictionary<string, string>
            {
                {"I", "Ⅰ"},
                {"II", "Ⅱ"},
                {"III", "Ⅲ"},
                {"IV", "Ⅳ"},
                {"V", "Ⅴ"},
                {"VI", "Ⅵ"},
                {"VII", "Ⅶ"},
                {"VIII", "Ⅷ"},
                {"IX", "Ⅸ"},
                {"X", "Ⅹ"},
                {"XI", "Ⅺ"},
                {"XII", "Ⅻ"},
                {"L", "Ⅼ"},
                {"C", "Ⅽ"},
                {"D", "Ⅾ"},
                {"M", "Ⅿ"},

                {"ⅠⅠ", "Ⅱ"},
                {"ⅠⅠⅠ", "Ⅲ"},

                {"ⅠⅤ", "Ⅳ"},

                {"ⅤⅠ", "Ⅵ"},
                {"ⅤⅡ", "Ⅶ"},
                {"ⅤⅢ", "Ⅷ"},

                {"ⅠⅩ", "Ⅸ"},

                {"ⅩⅠ", "Ⅺ"},
                {"ⅩⅡ", "Ⅻ"},
            }.WithVinculum()
            .Concat(new Dictionary<string, string>
            {
                {"(|)", "ↀ"},
                {"|))", "ↁ"},
                {"((|))", "ↂ"},
                {"|)))", "ↇ"},
                {"(((|)))", "ↈ"},
            });
    }

    private static IDictionary<string, string> CreateLigatures()
    {
        return new Dictionary<string, string>
            {
                {"ⅠⅠ", "Ⅱ"},
                {"ⅠⅡ", "Ⅲ"},
                {"ⅡⅠ", "Ⅲ"},
                {"ⅠⅠⅠ", "Ⅲ"},

                {"ⅠⅤ", "Ⅳ"},

                {"ⅤⅠ", "Ⅵ"},
                {"ⅤⅡ", "Ⅶ"},
                {"ⅤⅢ", "Ⅷ"},

                {"ⅠⅩ", "Ⅸ"},

                {"ⅩⅠ", "Ⅺ"},
                {"ⅩⅡ", "Ⅻ"},
            }.WithVinculum();
    }

    private static IEnumerable<Numeral> CreateDefaultNumerals()
    {
        return new Numeral[]
            {
                new("I", 1, NumeralKind.Any),
                new("V", 5, NumeralKind.Any),
                new("X", 10, NumeralKind.Any),
                new("L", 50, NumeralKind.Any),
                new("C", 100, NumeralKind.Any),
                new("D", 500, NumeralKind.Any),
            }.SelectMany(n => n.WithVinculum())
            .Concat(new Numeral[]
            {
                new("M", 1000, NumeralKind.Default),
                new("(|)", 1000, NumeralKind.Apostrophus, NumeralOptions.NoSubtract),
                new("|))", 5_000, NumeralKind.Apostrophus, NumeralOptions.NoSubtract),
                new("((|))", 10_000, NumeralKind.Apostrophus, NumeralOptions.NoSubtract),
                new("|)))", 50_000, NumeralKind.Apostrophus, NumeralOptions.NoSubtract),
                new("(((|)))", 100_000, NumeralKind.Apostrophus, NumeralOptions.NoSubtract),
            });
    }

    public NumeralsSet(uint @base, IEnumerable<Numeral> numerals, IDictionary<string, string> unicodeAliases, IDictionary<string, string> ligatures)
    {
        Base = @base;
        _numeralsByValue = numerals.GroupBy(n => n.Value).ToDictionary(n => n.Key, n => (ICollection<Numeral>)n.ToArray());
        _unicodeAliases = unicodeAliases.ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.Ordinal);
        _ligatures = ligatures.ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.Ordinal);
        MaximumLength = new[]
        {
            _numeralsByValue.Values.SelectMany(n => n).Select(n=>n.Literal.Length).Max(),
            _unicodeAliases.Keys.Select(n=>n.Length).Max(),
            _unicodeAliases.Values.Select(n=>n.Length).Max(),
            _ligatures.Keys.Select(n=>n.Length).Max(),
        }.Max();
    }

    private IDictionary<string, Numeral> CreateNumeralsByLiteral()
    {
        return _numeralsByValue.Values.SelectMany(n => n).GroupBy(n => n.Literal).ToDictionary(n => n.Key, n => n.Single());
    }

    public Numeral TryGetNumeral(string s)
    {
        if (NumeralsByLiteral.TryGetValue(s, out var n))
            return n;
        return null;
    }

    public Numeral TryGetNumeral(string s, int start, int length) => TryGetNumeral(s.Substring(start, length));

    public string Unligature(string s) => Replace(s, _ligatures);
    public string UnUnicode(string s) => Replace(s, _unicodeAliases);

    private static string Replace(string s, IDictionary<string, string> replacements)
    {
        for (; ; )
        {
            var s0 = s;
            foreach (var replacement in replacements)
            {
                s = s.Replace(replacement.Value, replacement.Key);
            }
            if (s == s0)
                return s;
        }
    }

    internal NumeralTriplet GetTriplet(uint pow, NumeralKind kind)
    {
        var nextPow = pow * Base;
        return new NumeralTriplet(_numeralsByValue.TryGetOrDefault(pow)?.FirstOrDefault(n => n.Matches(kind)),
            _numeralsByValue.TryGetOrDefault(nextPow / 2)?.FirstOrDefault(n => n.Matches(kind)),
            _numeralsByValue.TryGetOrDefault(nextPow)?.FirstOrDefault(n => n.Matches(kind)));
    }

    internal string GetUnicodeAlias(string s)
    {
        if (_unicodeAliases.TryGetValue(s, out var v))
            return v;
        return s;
    }

    internal string GetUnicodeLigature(string s)
    {
        for (; ; )
        {
            bool replaced = false;
            foreach (var unicodeAlias in _ligatures.OrderByDescending(kv => kv.Key.Length))
            {
                if (!s.EndsWith(unicodeAlias.Key, StringComparison.Ordinal))
                    continue;
                var start = s.Substring(0, s.Length - unicodeAlias.Key.Length);
                s = start + unicodeAlias.Value;
                replaced = true;
                break;
            }
            if (!replaced)
                break;
        }
        return s;
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

    public bool Unicode { get; set; } = false;
    public bool Ligature { get; set; } = false;

    public NumeralKind Kind { get; set; } = NumeralKind.Default;

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

    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        throw new NotImplementedException();
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
