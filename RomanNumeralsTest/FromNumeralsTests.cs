using NUnit.Framework;
using RomanNumerals;

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
        var v = literalRoman.FromRomanNumerals();
        Assert.AreEqual(expectedValue, v);
    }
}