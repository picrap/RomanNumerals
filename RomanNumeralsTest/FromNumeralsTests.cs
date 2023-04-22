using NUnit.Framework;
using RomanNumerals.Numerals;

namespace RomanNumeralsTest;

[TestFixture]
public class FromNumeralsTests
{
    [Test]
    [TestCase("I", 1u)]
    [TestCase("MMXIX", 2019u)]
    [TestCase("MDCLXVI", 1666u)]
    [TestCase("CDXLIV", 444u)]
    public void ParseTest(string literalRoman, uint expectedValue)
    {
        var p = NumeralParser.TryParse(literalRoman, out var v);
        Assert.IsTrue(p);
        Assert.AreEqual(expectedValue, v);
    }
}