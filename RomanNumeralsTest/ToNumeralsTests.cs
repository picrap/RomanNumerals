using NUnit.Framework;
using RomanNumerals.Numerals;
using NumeralBuilder = RomanNumerals.Numerals.NumeralBuilder;

namespace RomanNumeralsTest;

[TestFixture]
[Property("Direction", "ToString")]
public class ToNumeralsTests
{
    [Test]
    [Property("Class", "ASCII")]
    [TestCase(1u, "I")]
    [TestCase(10u, "X")]
    [TestCase(44u, "XLIV")]
    [TestCase(2019u, "MMXIX")]
    [TestCase(4999u, "MMMMCMXCIX")]
    public void AsciiTest(uint value, string expectedRoman)
    {
        var builder = new NumeralBuilder();
        var l = builder.ToString(value);
        Assert.AreEqual(expectedRoman, l);
    }

    [Test]
    [Property("Class", "ASCII")]
    [TestCase(4u, "IIII")]
    [TestCase(49u, "XXXXVIIII")]
    public void NoNegativeAsciiTest(uint value, string expectedRoman)
    {
        var builder = new NumeralBuilder(options: new NumeralBuilderOptions { SubtractableDigits = 0 });
        var l = builder.ToString(value);
        Assert.AreEqual(expectedRoman, l);
    }

    [Test]
    [Property("Class", "ASCII")]
    [Property("Extension", "Vinculum")]
    [TestCase(6005u, "V\u0305I\u0305V")]
    [TestCase(2_000_010u, "I\u033FI\u033FX")]
    public void VinculumTest(uint value, string expectedRoman)
    {
        var builder = new NumeralBuilder(options: new NumeralBuilderOptions { Kind = NumeralKind.Vinculum });
        var l = builder.ToString(value);
        Assert.AreEqual(expectedRoman, l);
    }

    [Test]
    [Property("Class", "Unicode")]
    [TestCase(1u, "Ⅰ")]
    [TestCase(12u, "Ⅻ")]
    [TestCase(227u, "ⅭⅭⅩⅩⅦ")]
    public void UnicodeTest(uint value, string expectedRoman)
    {
        var builder = new NumeralBuilder(options: new NumeralBuilderOptions { Unicode = true, Ligature = true });
        var l = builder.ToString(value);
        Assert.AreEqual(expectedRoman, l);
    }

    [Test]
    [Property("Class", "ASCII")]
    [Property("Extension", "Apostrophus")]
    [TestCase(1000u, "(|)")]
    [TestCase(2000u, "(|)(|)")]
    [TestCase(2001u, "(|)(|)I")]
    [TestCase(4900u, "(|)(|)(|)(|)DCCCC")]
    [TestCase(5000u, "|))")]
    [TestCase(5010u, "|))X")]
    public void ApostrophusTest(uint value, string expectedRoman)
    {
        var builder = new NumeralBuilder(options: new NumeralBuilderOptions { Kind = NumeralKind.Apostrophus });
        var l = builder.ToString(value);
        Assert.AreEqual(expectedRoman, l);
    }

    [Test]
    [Property("Class", "Unicode")]
    [Property("Extension", "Apostrophus")]
    [TestCase(1001u, "ↀⅠ")]
    [TestCase(5002u, "ↁⅡ")]
    [TestCase(200_012u, "ↈↈⅫ")]
    public void UnicodeApostrophusTest(uint value, string expectedRoman)
    {
        var builder = new NumeralBuilder(options: new NumeralBuilderOptions { Kind = NumeralKind.Apostrophus, Unicode = true, Ligature = true });
        var l = builder.ToString(value);
        Assert.AreEqual(expectedRoman, l);
    }
}
