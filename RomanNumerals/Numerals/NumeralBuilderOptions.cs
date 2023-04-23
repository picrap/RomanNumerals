namespace RomanNumerals.Numerals;

public class NumeralBuilderOptions
{
    public int SubtractableDigits { get; set; } = 1;

    public NumeralBuilderStyle Style { get; set; } = NumeralBuilderStyle.None;

    public NumeralKind Kind { get; set; } = NumeralKind.Default;

    public static NumeralBuilderOptions Default => new NumeralBuilderOptions();
}
