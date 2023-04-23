namespace RomanNumerals.Numerals;

internal class NumeralTriplet
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