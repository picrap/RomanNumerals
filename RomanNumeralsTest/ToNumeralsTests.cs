using NUnit.Framework;
using RomanNumerals;
using RomanNumerals.Numerals;

namespace RomanNumeralsTest;

[TestFixture]
public class ToNumeralsTests
{
    [Test]
    [TestCase(1u, "I")]
    [TestCase(10u, "X")]
    [TestCase(2019u, "MMXIX")]
    public void AsciiTest(uint value, string expectedRoman)
    {
        var l = value.ToRomanNumerals();
        Assert.AreEqual(expectedRoman, l);
    }

    [Test]
    [TestCase(6005u, "V\u0305I\u0305V")]
    [TestCase( 2_000_010u, "I\u033FI\u033FX")]
    public void VinculumTest(uint value, string expectedRoman)
    {
        var l = value.ToRomanNumerals(NumeralFlags.Vinculum);
        Assert.AreEqual(expectedRoman, l);
    }

    [Test]
    [TestCase(1u, "\u2160")]
    [TestCase(12u, "\u216B")]
    [TestCase(227u, "\u216D\u216D\u2169\u2169\u2166")]
    public void UnicodeTest(uint value, string expectedRoman)
    {
        var l = value.ToRomanNumerals(NumeralFlags.Unicode);
        Assert.AreEqual(expectedRoman, l);
    }
}