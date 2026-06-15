using System;
using NUnit.Framework;
using RomanNumerals;
using RomanNumerals.Numerals;

namespace RomanNumeralsTest;

[TestFixture]
[Property("Direction", "FromString")]
public class FromNumeralsTests
{
    [Test]
    [Property("Class", "Invalid")]
    [TestCase("J")]
    [TestCase("MMXIY")]
    [TestCase("MDCHXVI")]
    public void ParseInvaledTest(string literalRoman)
    {
        Assert.IsNull(literalRoman.TryFromRomanNumerals());
    }

    [Test]
    [Property("Class", "ASCII")]
    [TestCase("I", 1u)]
    [TestCase("MMXIX", 2019u)]
    [TestCase("MDCLXVI", 1666u)]
    [TestCase("CDXLIV", 444u)]
    public void ParseAsciiTest(string literalRoman, uint expectedValue)
    {
        var v = literalRoman.FromRomanNumerals();
        Assert.AreEqual(expectedValue, v);
    }

    [Test]
    [Property("Class", "ASCII")]
    [Property("Extension", "Vinculum")]
    [TestCase("V\u0305I\u0305V", 6005u)]
    [TestCase("I\u033FI\u033FX", 2_000_010u)]
    public void ParseVinculumTest(string literalRoman, uint expectedValue)
    {
        var v = literalRoman.FromRomanNumerals();
        Assert.AreEqual(expectedValue, v);
    }

    [Test]
    [Property("Class", "Unicode")]
    [TestCase("Ⅰ", 1u)]
    [TestCase("Ⅻ", 12u)]
    [TestCase("ⅭⅭⅩⅩⅦ", 227u)]
    public void ParseUnicodeTest(string literalRoman, uint expectedValue)
    {
        var v = literalRoman.FromRomanNumerals();
        Assert.AreEqual(expectedValue, v);
    }

    [Test]
    [Property("Class", "Unicode")]
    [Property("Extension", "Apostrophus")]
    [TestCase("ↀⅠ", 1001u)]
    [TestCase("ↁⅡ", 5002u)]
    [TestCase("ↈↈⅫ", 200_012u)]
    public void ParseUnicodeApostrophusTest(string literalRoman, uint expectedValue)
    {
        var v = literalRoman.FromRomanNumerals();
        Assert.AreEqual(expectedValue, v);
    }

    [Test]
    [Property("Class", "Bastards 😣")]
    [TestCase("ⅠI", 2u)]
    [TestCase("ⅫⅠ", 13u)]
    [TestCase("ⅭCⅩXⅦ", 227u)]
    public void ParseBastardsTest(string literalRoman, uint expectedValue)
    {
        var v = literalRoman.FromRomanNumerals();
        Assert.AreEqual(expectedValue, v);
    }

    [Test]
    [Property("Class", "AsciiOnly")]
    [TestCase("I", 1u)]
    [TestCase("MMXIX", 2019u)]
    [TestCase("MDCLXVI", 1666u)]
    [TestCase("CDXLIV", 444u)]
    [TestCase("V\u0305I\u0305V", null)]
    [TestCase("Ⅻ", null)]
    [TestCase("ↀⅠ", null)]
    public void ParseAsciiOnlyTest(string literalRoman, uint? expectedValue)
    {
        var numeralsSet =
            new NumeralsSet(
                new Numeral[]
                {
                    new("I", 1U, NumeralKind.Any),
                    new("V", 5U, NumeralKind.Any),
                    new("X", 10U, NumeralKind.Any),
                    new("L", 50U, NumeralKind.Any),
                    new("C", 100U, NumeralKind.Any),
                    new("D", 500U, NumeralKind.Any),
                    new("M", 1000U, NumeralKind.Any),
                }
            );
        var v = NumeralParser.Default.TryParse(literalRoman, out var x, numeralsSet);
        Assert.AreEqual(expectedValue, v ? x : null);
    }
}