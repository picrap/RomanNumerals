namespace RomanNumerals.Numerals
{
    using System.Collections.Generic;
    using System.Linq;

    public static class RomanNumeralsDefinition
    {
        public static readonly LiteralNumeral[] LiteralNumerals = new[]
        {
            new LiteralNumeral("I",1),
            new LiteralNumeral("II",2),
            new LiteralNumeral("III",3),
            new LiteralNumeral("IV",4),
            new LiteralNumeral("V",5),
            new LiteralNumeral("VI",6),
            new LiteralNumeral("VII",7),
            new LiteralNumeral("VIII",8),
            new LiteralNumeral("IX",9),

            new LiteralNumeral("X",10),
            new LiteralNumeral("XX",20),
            new LiteralNumeral("XXX",30),
            new LiteralNumeral("XL",40),
            new LiteralNumeral("L",50),
            new LiteralNumeral("LX",60),
            new LiteralNumeral("LXX",70),
            new LiteralNumeral("LXXX",80),
            new LiteralNumeral("XC",90),

            new LiteralNumeral("C",100),
            new LiteralNumeral("CC",200),
            new LiteralNumeral("CCC",300),
            new LiteralNumeral("CD",400),
            new LiteralNumeral("D",500),
            new LiteralNumeral("DC",600),
            new LiteralNumeral("DCC",700),
            new LiteralNumeral("DCCC",800),
            new LiteralNumeral("CM",900),

            new LiteralNumeral("M",1000),
            new LiteralNumeral("MM",2000),
            new LiteralNumeral("MMM",3000),
            new LiteralNumeral("MMMM",4000),

            new LiteralNumeral("\u2160",1),
            new LiteralNumeral("\u2161",2),
            new LiteralNumeral("\u2162",3),
            new LiteralNumeral("\u2163",4),
            new LiteralNumeral("\u2164",5),
            new LiteralNumeral("\u2165",6),
            new LiteralNumeral("\u2166",7),
            new LiteralNumeral("\u2167",8),
            new LiteralNumeral("\u2168",9),

            new LiteralNumeral("\u2169",10),
            new LiteralNumeral("\u2169\u2169",20),
            new LiteralNumeral("\u2169\u2169\u2169",30),
            new LiteralNumeral("\u2169\u216C",40),
            new LiteralNumeral("\u216C",50),
            new LiteralNumeral("\u216C\u2169",60),
            new LiteralNumeral("\u216C\u2169\u2169",70),
            new LiteralNumeral("\u216C\u2169\u2169\u2169",80),
            new LiteralNumeral("\u2169\u216D",90),

            new LiteralNumeral("\u216D",100),
            new LiteralNumeral("\u216D\u216D",200),
            new LiteralNumeral("\u216D\u216D\u216D",300),
            new LiteralNumeral("\u216D\u216E",400),
            new LiteralNumeral("\u216E",500),
            new LiteralNumeral("\u216E\u216D",600),
            new LiteralNumeral("\u216E\u216D\u216D",700),
            new LiteralNumeral("\u216E\u216D\u216D\u216D",800),
            new LiteralNumeral("\u216D\u216F",900),

            new LiteralNumeral("\u216F",1000),
            new LiteralNumeral("\u216F\u216F",2000),
            new LiteralNumeral("\u216F\u216F\u216F",3000),
            new LiteralNumeral("\u216F\u216F\u216F\u216F",4000),
        };

        public static LiteralNumeral TryGet(uint digit, NumeralFlags flags)
        {
            return LiteralNumerals.FirstOrDefault(v => v.Digit == digit && v.Matches(flags));
        }
    }
}