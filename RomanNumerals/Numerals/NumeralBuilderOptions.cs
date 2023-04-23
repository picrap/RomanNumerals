namespace RomanNumerals.Numerals;

public class NumeralBuilderOptions
{
    public int SubtractableDigits { get; set; } = 1;

    public NumeralBuilderStyle Style { get; set; } = NumeralBuilderStyle.None;

    public NumeralKind Kind { get; set; } = NumeralKind.Default;

    public static NumeralBuilderOptions Default => new NumeralBuilderOptions();

    public static NumeralBuilderOptions FromFormatString(string formatString)
    {
        var options = new NumeralBuilderOptions();
        if (formatString is not null)
        {
            foreach (var c in formatString)
            {
                switch (c)
                {
                    case '0':
                        options.SubtractableDigits = 0;
                        break;
                    case 'V':
                    case '-':
                    case '=':
                        options.Kind = NumeralKind.Vinculum;
                        break;
                    case '\'':
                    case '|':
                        options.Kind = NumeralKind.Apostrophus;
                        break;
                    case 'U':
                        options.Style = options.Style | NumeralBuilderStyle.Unicode | NumeralBuilderStyle.Ligature;
                        break;
                    case 'u':
                        options.Style = (options.Style & ~NumeralBuilderStyle.Ligature) | NumeralBuilderStyle.Unicode;
                        break;
                    case 'A':
                        options.Style = options.Style & ~NumeralBuilderStyle.Ligature | NumeralBuilderStyle.Unicode;
                        break;
                }
            }
        }
        return options;
    }
}
