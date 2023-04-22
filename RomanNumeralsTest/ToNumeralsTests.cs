using NUnit.Framework;
using RomanNumerals;
using RomanNumerals.Numerals;
using NumeralBuilder = RomanNumerals.NumeralBuilder;

namespace RomanNumeralsTest;

[TestFixture]
public class ToNumeralsTests
{
    [Test]
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
    [TestCase(4u, "IIII")]
    [TestCase(49u, "XXXXVIIII")]
    public void NoNegativeAsciiTest(uint value, string expectedRoman)
    {
        var builder = new NumeralBuilder(options: new NumeralBuilderOptions { SubtractableDigits = 0 });
        var l = builder.ToString(value);
        Assert.AreEqual(expectedRoman, l);
    }

    [Test]
    [TestCase(6005u, "V\u0305I\u0305V")]
    [TestCase(2_000_010u, "I\u033FI\u033FX")]
    public void VinculumTest(uint value, string expectedRoman)
    {
        var builder = new NumeralBuilder(options: new NumeralBuilderOptions { Flags = NumeralFlags.Vinculum });
        var l = builder.ToString(value);
        Assert.AreEqual(expectedRoman, l);
    }

    [Test]
    [TestCase(1u, "Ⅰ")]
    [TestCase(12u, "Ⅻ")]
    [TestCase(227u, "ⅭⅭⅩⅩⅦ")]
    public void UnicodeTest(uint value, string expectedRoman)
    {
        var builder = new NumeralBuilder(options: new NumeralBuilderOptions { Flags = NumeralFlags.Unicode });
        var l = builder.ToString(value);
        Assert.AreEqual(expectedRoman, l);
    }
}