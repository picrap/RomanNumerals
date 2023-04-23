namespace RomanNumerals.Numerals;

public class NumeralBuilderOptions
{
    public int SubtractableDigits { get; set; } = 1;

    public bool Unicode { get; set; } = false;
    public bool Ligature { get; set; } = false;

    public NumeralKind Kind { get; set; } = NumeralKind.Default;

    public static NumeralBuilderOptions Default => new NumeralBuilderOptions();
}