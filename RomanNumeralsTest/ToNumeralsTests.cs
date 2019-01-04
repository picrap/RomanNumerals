namespace RomanNumeralsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RomanNumerals.Numerals;

    [TestClass]
    public class ToNumeralsTests
    {
        [TestMethod]
        public void Convert1ToITest()
        {
            var l = NumeralBuilder.Build(1);
            Assert.AreEqual("I", l);
        }

        [TestMethod]
        public void Convert10ToXTest()
        {
            var l = NumeralBuilder.Build(10);
            Assert.AreEqual("X", l);
        }

        [TestMethod]
        public void Convert2019ToMMXIXTest()
        {
            var l = NumeralBuilder.Build(2019);
            Assert.AreEqual("MMXIX", l);
        }

        [TestMethod]
        public void Convert1ToⅠTest()
        {
            var l = NumeralBuilder.Build(1, NumeralFlags.Unicode);
            Assert.AreEqual("\u2160", l);
        }

        [TestMethod]
        public void Convert12ToⅫTest()
        {
            var l = NumeralBuilder.Build(12, NumeralFlags.Unicode);
            Assert.AreEqual("\u216B", l);
        }

    }
}