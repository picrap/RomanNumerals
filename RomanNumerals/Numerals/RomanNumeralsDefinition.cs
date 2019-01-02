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
        };

        public static LiteralNumeral TryGet(uint digit)
        {
            return LiteralNumerals.FirstOrDefault(v => v.Digit == digit);
        }

        public static IEnumerable<LiteralNumeral> GetOrderedNumerals(int log10)
        {
            return OrderedNumerals.Where(n => n.Log10 == log10);
        }

        public static IEnumerable<LiteralNumeral> OrderedNumerals
        {
            get { return LiteralNumerals.OrderByDescending(n => n.Digit); }
        }
    }
}