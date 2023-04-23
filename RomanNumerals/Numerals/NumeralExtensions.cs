using System.Collections.Generic;
using System.Linq;
using System.Text;
using RomanNumerals.Utility;

namespace RomanNumerals.Numerals;

internal static class NumeralExtensions
{
    public static IEnumerable<Numeral> WithVinculum(this Numeral numeral)
    {
        yield return numeral;
        yield return numeral.CreateVinculum('\u0305', 1_000);
        yield return numeral.CreateVinculum('\u033F', 1_000_000);
    }

    public static IDictionary<string, string> WithVinculum(this IDictionary<string, string> aliases)
    {
        return aliases
            .Concat(aliases.Select(kv => CreateVinculum((KeyValuePair<string, string>) kv, '\u0305')))
            .Concat(aliases.Select(kv => kv.CreateVinculum('\u033F')));
    }

    public static IEnumerable<Numeral> Single(this Numeral numeral)
    {
        yield return numeral;
    }

    private static Numeral CreateVinculum(this Numeral numeral, char marker, uint factor)
    {
        return new Numeral(numeral.Literal.CreateVinculum(marker), numeral.Value * factor, NumeralKind.Vinculum);
    }

    private static KeyValuePair<string, string> CreateVinculum(this KeyValuePair<string, string> kv, char marker)
    {
        return new(kv.Key.CreateVinculum(marker), kv.Value.CreateVinculum(marker));
    }

    private static string CreateVinculum(this string s, char marker)
    {
        var v = new StringBuilder();
        foreach (var c in s)
            v.AppendFormat("{0}{1}", c, marker);
        return v.ToString();
    }
}