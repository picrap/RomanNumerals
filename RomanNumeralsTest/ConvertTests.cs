using NUnit.Framework;
using RomanNumerals.Numerals;

namespace RomanNumeralsTest;

[TestFixture]
[Property("Direction", "ToString")]
public class ConvertTests
{
    [Test]
    [Property("Class", "CustomFormatter")]
    [TestCase(1u, null, "I")]
    [TestCase(1u, "U", "Ⅰ")]
    [TestCase(3u, "u", "ⅠⅠⅠ")]
    [TestCase(3u, "U", "Ⅲ")]
    public void CustomFormatterTest(uint value, string format, string expected)
    {
        var s = NumeralBuilder.Default.Format(format, value, null);
        Assert.AreEqual(expected, s);
    }
}
