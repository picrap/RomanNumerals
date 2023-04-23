using System;
using System.Collections.Generic;
using System.Linq;
using RomanNumerals.Utility;

namespace RomanNumerals.Numerals;

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
            }.SelectMany(n => NumeralExtensions.WithVinculum((Numeral) n))
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