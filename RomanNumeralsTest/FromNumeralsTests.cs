﻿namespace RomanNumeralsTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RomanNumerals.Numerals;

    [TestClass]
    public class FromNumeralsTests
    {
        [TestMethod]
        public void ConvertITo1Test()
        {
            var p = NumeralParser.TryParse("I", out var v);
            Assert.IsTrue(p);
            Assert.AreEqual(1u, v);
        }

        [TestMethod]
        public void ConvertMMXIXTo2019Test()
        {
            var p = NumeralParser.TryParse("MMXIX", out var v);
            Assert.IsTrue(p);
            Assert.AreEqual(2019u, v);
        }

        [TestMethod]
        public void ConvertMDCLXVITo1666Test()
        {
            var p = NumeralParser.TryParse("MDCLXVI", out var v);
            Assert.IsTrue(p);
            Assert.AreEqual(1666u, v);
        }

        [TestMethod]
        public void ConvertCDXLIVTo444Test()
        {
            var p = NumeralParser.TryParse("CDXLIV", out var v);
            Assert.IsTrue(p);
            Assert.AreEqual(444u, v);
        }
    }
}