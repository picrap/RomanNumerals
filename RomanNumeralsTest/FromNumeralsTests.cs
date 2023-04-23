using NUnit.Framework;
using RomanNumerals;

namespace RomanNumeralsTest;

[TestFixture]
[Property("Direction", "FromString")]
public class FromNumeralsTests
{
    [Test]
    [Property("Class", "ASCII")]
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