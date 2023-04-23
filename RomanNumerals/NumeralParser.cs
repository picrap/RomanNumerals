using System;
using System.Collections.Generic;
using System.Linq;

namespace RomanNumerals;

public class NumeralParser
{
    public static readonly NumeralParser Default = new();

    private class NumeralCount
    {
        public Numeral Numeral;
        public int Count;
        public long Value => Numeral.Value * Count;

        public NumeralCount(ICollection<Numeral> numerals)
        {
            Numeral = numerals.First();
            Count = numerals.Count;
        }
    }

    public bool TryParse(string romanNumeral, out uint v, NumeralsSet numeralsSet = null)
    {
        numeralsSet ??= NumeralsSet.Default;
        var unicodeNumeral = numeralsSet.Unligature(romanNumeral);
        var asciiNumeral = numeralsSet.UnUnicode(unicodeNumeral);
        long value = 0;
        NumeralCount currentNumerals = null;
        foreach (var numerals in GetGroupedNumerals(asciiNumeral, numeralsSet))
        {
            if (numerals is null)
            {
                v = 0;
                return false;
            }

            if (currentNumerals is not null)
            {
                if (currentNumerals.Numeral.Value < numerals.Numeral.Value)
                    value -= currentNumerals.Value;
                else
                    value += currentNumerals.Value;
            }

            currentNumerals = numerals;
        }
        if (currentNumerals is not null)
            value += currentNumerals.Value;

        if (value < 0)
        {
            v = 0;
            return false;
        }
        v = (uint)value;
        return true;
    }

    private static IEnumerable<NumeralCount?> GetGroupedNumerals(string s, NumeralsSet numeralsSet)
    {
        uint currentValue = uint.MaxValue;
        var values = new List<Numeral>();
        foreach (var numeral in GetNumerals(s, numeralsSet))
        {
            if (numeral is null)
            {
                yield return null;
                yield break;
            }
            if (numeral.Value != currentValue)
            {
                if (values.Count > 0)
                    yield return new(values);
                values.Clear();
                currentValue = numeral.Value;
            }
            values.Add(numeral);
        }
        if (values.Count > 0)
            yield return new(values);
    }

    private static IEnumerable<Numeral> GetNumerals(string s, NumeralsSet numeralsSet)
    {
        for (int startIndex = 0; startIndex < s.Length;)
        {
            var numeral = FindNumeral(s, numeralsSet, ref startIndex);
            yield return numeral;
            if (numeral is null)
                yield break;
        }
    }

    private static Numeral FindNumeral(string s, NumeralsSet numeralsSet, ref int startIndex)
    {
        var remainingLength = Math.Min(s.Length - startIndex, numeralsSet.MaximumLength);
        for (int length = 1; length <= remainingLength; length++)
        {
            var numeral = numeralsSet.TryGetNumeral(s, startIndex, length);
            if (numeral is null)
                continue;
            startIndex += length;
            return numeral;
        }

        return null;
    }
}
