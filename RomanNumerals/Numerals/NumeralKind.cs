namespace RomanNumerals.Numerals;

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