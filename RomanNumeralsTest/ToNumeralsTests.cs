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
    [TestCase(1u, "Ⅰ")]
    [TestCase(12u, "Ⅻ")]
    [TestCase(13u, "ⅩⅢ")]
    [TestCase(227u, "ⅭⅭⅩⅩⅦ")]
    public void UnicodeTest(uint value, string expectedRoman)
    {
        var l = value.ToRomanNumerals(NumeralFlags.Unicode);
        Assert.AreEqual(expectedRoman, l);
    }
}