using System.Collections.Generic;
using System.Linq;
using System.Text;
using RomanNumerals.Utility;

namespace RomanNumerals.Numerals;

/// <summary>
///     Definitions for numerals
/// </summary>
/// <remarks>
///     It starts from a simple list (<see cref="BaseLiteralNumerals" />) and vinculum numerals are added later
/// </remarks>
public static class RomanNumeralsDefinition
{
    private static readonly LiteralNumeral[] BaseLiteralNumerals =
    {
        // ASCII

        new("I", 1, NumeralFlags.Ascii),
        new("II", 2, NumeralFlags.Ascii),
        new("III", 3, NumeralFlags.Ascii),
        new("IV", 4, NumeralFlags.Ascii),
        new("V", 5, NumeralFlags.Ascii),
        new("VI", 6, NumeralFlags.Ascii),
        new("VII", 7, NumeralFlags.Ascii),
        new("VIII", 8, NumeralFlags.Ascii),
        new("IX", 9, NumeralFlags.Ascii),

        new("X", 10, NumeralFlags.Ascii),
        new("XX", 20, NumeralFlags.Ascii),
        new("XXX", 30, NumeralFlags.Ascii),
        new("XL", 40, NumeralFlags.Ascii),
        new("L", 50, NumeralFlags.Ascii),
        new("LX", 60, NumeralFlags.Ascii),
        new("LXX", 70, NumeralFlags.Ascii),
        new("LXXX", 80, NumeralFlags.Ascii),
        new("XC", 90, NumeralFlags.Ascii),

        new("C", 100, NumeralFlags.Ascii),
        new("CC", 200, NumeralFlags.Ascii),
        new("CCC", 300, NumeralFlags.Ascii),
        new("CD", 400, NumeralFlags.Ascii),
        new("D", 500, NumeralFlags.Ascii),
        new("DC", 600, NumeralFlags.Ascii),
        new("DCC", 700, NumeralFlags.Ascii),
        new("DCCC", 800, NumeralFlags.Ascii),
        new("CM", 900, NumeralFlags.Ascii),

        new("M", 1000, NumeralFlags.Ascii),
        new("MM", 2000, NumeralFlags.Ascii),
        new("MMM", 3000, NumeralFlags.Ascii),
        new("MMMM", 4000, NumeralFlags.Ascii),

        // Unicode

        new("Ⅰ", 1, NumeralFlags.Unicode),
        new("Ⅱ", 2, NumeralFlags.Unicode),
        new("Ⅲ", 3, NumeralFlags.Unicode),
        new("Ⅳ", 4, NumeralFlags.Unicode),
        new("Ⅴ", 5, NumeralFlags.Unicode),
        new("Ⅵ", 6, NumeralFlags.Unicode),
        new("Ⅶ", 7, NumeralFlags.Unicode),
        new("Ⅷ", 8, NumeralFlags.Unicode),
        new("Ⅸ", 9, NumeralFlags.Unicode),

        new("Ⅹ", 10, NumeralFlags.Unicode),
        new("Ⅺ", 11, NumeralFlags.Unicode),
        new("Ⅻ", 12, NumeralFlags.Unicode),
        new("ⅩⅩ", 20, NumeralFlags.Unicode),
        new("ⅩⅩⅩ", 30, NumeralFlags.Unicode),
        new("ⅩⅬ", 40, NumeralFlags.Unicode),
        new("Ⅼ", 50, NumeralFlags.Unicode),
        new("ⅬⅩ", 60, NumeralFlags.Unicode),
        new("ⅬⅩⅩ", 70, NumeralFlags.Unicode),
        new("ⅬⅩⅩⅩ", 80, NumeralFlags.Unicode),
        new("ⅩⅭ", 90, NumeralFlags.Unicode),

        new("Ⅽ", 100, NumeralFlags.Unicode),
        new("ⅭⅭ", 200, NumeralFlags.Unicode),
        new("ⅭⅭⅭ", 300, NumeralFlags.Unicode),
        new("ⅭⅮ", 400, NumeralFlags.Unicode),
        new("Ⅾ", 500, NumeralFlags.Unicode),
        new("ⅮⅭ", 600, NumeralFlags.Unicode),
        new("ⅮⅭⅭ", 700, NumeralFlags.Unicode),
        new("ⅮⅭⅭⅭ", 800, NumeralFlags.Unicode),
        new("ⅭⅯ", 900, NumeralFlags.Unicode),

        new("Ⅿ", 1000, NumeralFlags.Unicode),
        new("ⅯⅯ", 2000, NumeralFlags.Unicode),
        new("ⅯⅯⅯ", 3000, NumeralFlags.Unicode),
        new("ⅯⅯⅯⅯ", 4000, NumeralFlags.Unicode)
    };

    private static IDictionary<uint, IList<LiteralNumeral>> _literalNumerals;

    internal static IDictionary<uint, IList<LiteralNumeral>> LiteralNumerals => _literalNumerals ??= CreateLiteralNumerals();

    private static IDictionary<uint, IList<LiteralNumeral>> CreateLiteralNumerals()
    {
        var literalNumerals = new Dictionary<uint, IList<LiteralNumeral>>();
        foreach (var baseLiteralNumeral in BaseLiteralNumerals)
        {
            AddLiteralNumeral(literalNumerals, baseLiteralNumeral);
            if ((baseLiteralNumeral.Flags == NumeralFlags.Ascii || baseLiteralNumeral.Flags == NumeralFlags.Unicode)
                && baseLiteralNumeral.Digit < 1000)
            {
                AddLiteralNumeral(literalNumerals, CreateVinculum(baseLiteralNumeral, '\u0305', 1000));
                AddLiteralNumeral(literalNumerals, CreateVinculum(baseLiteralNumeral, '\u033F', 1000000));
            }
        }

        return literalNumerals;
    }

    /// <summary>
    ///     Adds a literal numeral
    /// </summary>
    /// <param name="literalNumerals"></param>
    /// <param name="literalNumeral"></param>
    private static void AddLiteralNumeral(IDictionary<uint, IList<LiteralNumeral>> literalNumerals, LiteralNumeral literalNumeral)
    {
        literalNumerals.TryGetOrAddNew(literalNumeral.Digit, () => new List<LiteralNumeral>()).Add(literalNumeral);
    }

    private static LiteralNumeral CreateVinculum(LiteralNumeral numeral, char marker, int factor)
    {
        var literal = new StringBuilder();
        foreach (var c in numeral.Literal)
            literal.AppendFormat("{0}{1}", c, marker);
        return new LiteralNumeral(literal.ToString(), (uint)(numeral.Digit * factor), numeral.Flags | NumeralFlags.Vinculum);
    }

    /// <summary>
    ///     Tries to get a numeral matching the digit and flags
    /// </summary>
    /// <param name="digit"></param>
    /// <param name="flags"></param>
    /// <returns></returns>
    public static LiteralNumeral TryGet(uint digit, NumeralFlags flags)
    {
        if (!LiteralNumerals.TryGetValue(digit, out var numerals))
            return null;
        return numerals.FirstOrDefault(v => v.Matches(flags));
    }
}