using System.Diagnostics;

namespace RomanNumerals.Numerals;

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