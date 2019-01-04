namespace RomanNumerals.Numerals
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class RomanNumeralsDefinition
    {
        private static readonly LiteralNumeral[] BaseLiteralNumerals =
        {
            new LiteralNumeral("I",1,NumeralFlags.Ascii),
            new LiteralNumeral("II",2,NumeralFlags.Ascii),
            new LiteralNumeral("III",3,NumeralFlags.Ascii),
            new LiteralNumeral("IV",4,NumeralFlags.Ascii),
            new LiteralNumeral("V",5,NumeralFlags.Ascii),
            new LiteralNumeral("VI",6,NumeralFlags.Ascii),
            new LiteralNumeral("VII",7,NumeralFlags.Ascii),
            new LiteralNumeral("VIII",8,NumeralFlags.Ascii),
            new LiteralNumeral("IX",9,NumeralFlags.Ascii),

            new LiteralNumeral("X",10,NumeralFlags.Ascii),
            new LiteralNumeral("XX",20,NumeralFlags.Ascii),
            new LiteralNumeral("XXX",30,NumeralFlags.Ascii),
            new LiteralNumeral("XL",40,NumeralFlags.Ascii),
            new LiteralNumeral("L",50,NumeralFlags.Ascii),
            new LiteralNumeral("LX",60,NumeralFlags.Ascii),
            new LiteralNumeral("LXX",70,NumeralFlags.Ascii),
            new LiteralNumeral("LXXX",80,NumeralFlags.Ascii),
            new LiteralNumeral("XC",90,NumeralFlags.Ascii),

            new LiteralNumeral("C",100,NumeralFlags.Ascii),
            new LiteralNumeral("CC",200,NumeralFlags.Ascii),
            new LiteralNumeral("CCC",300,NumeralFlags.Ascii),
            new LiteralNumeral("CD",400,NumeralFlags.Ascii),
            new LiteralNumeral("D",500,NumeralFlags.Ascii),
            new LiteralNumeral("DC",600,NumeralFlags.Ascii),
            new LiteralNumeral("DCC",700,NumeralFlags.Ascii),
            new LiteralNumeral("DCCC",800,NumeralFlags.Ascii),
            new LiteralNumeral("CM",900,NumeralFlags.Ascii),

            new LiteralNumeral("M",1000,NumeralFlags.Ascii),
            new LiteralNumeral("MM",2000,NumeralFlags.Ascii),
            new LiteralNumeral("MMM",3000,NumeralFlags.Ascii),
            new LiteralNumeral("MMMM",4000,NumeralFlags.Ascii),

            new LiteralNumeral("\u2160",1, NumeralFlags.Unicode),
            new LiteralNumeral("\u2161",2, NumeralFlags.Unicode),
            new LiteralNumeral("\u2162",3, NumeralFlags.Unicode),
            new LiteralNumeral("\u2163",4, NumeralFlags.Unicode),
            new LiteralNumeral("\u2164",5, NumeralFlags.Unicode),
            new LiteralNumeral("\u2165",6, NumeralFlags.Unicode),
            new LiteralNumeral("\u2166",7, NumeralFlags.Unicode),
            new LiteralNumeral("\u2167",8, NumeralFlags.Unicode),
            new LiteralNumeral("\u2168",9, NumeralFlags.Unicode),

            new LiteralNumeral("\u2169",10, NumeralFlags.Unicode),
            new LiteralNumeral("\u216A",11, NumeralFlags.Unicode),
            new LiteralNumeral("\u216B",12, NumeralFlags.Unicode),
            new LiteralNumeral("\u2169\u2169",20, NumeralFlags.Unicode),
            new LiteralNumeral("\u2169\u2169\u2169",30, NumeralFlags.Unicode),
            new LiteralNumeral("\u2169\u216C",40, NumeralFlags.Unicode),
            new LiteralNumeral("\u216C",50, NumeralFlags.Unicode),
            new LiteralNumeral("\u216C\u2169",60, NumeralFlags.Unicode),
            new LiteralNumeral("\u216C\u2169\u2169",70, NumeralFlags.Unicode),
            new LiteralNumeral("\u216C\u2169\u2169\u2169",80, NumeralFlags.Unicode),
            new LiteralNumeral("\u2169\u216D",90, NumeralFlags.Unicode),

            new LiteralNumeral("\u216D",100, NumeralFlags.Unicode),
            new LiteralNumeral("\u216D\u216D",200, NumeralFlags.Unicode),
            new LiteralNumeral("\u216D\u216D\u216D",300, NumeralFlags.Unicode),
            new LiteralNumeral("\u216D\u216E",400, NumeralFlags.Unicode),
            new LiteralNumeral("\u216E",500, NumeralFlags.Unicode),
            new LiteralNumeral("\u216E\u216D",600, NumeralFlags.Unicode),
            new LiteralNumeral("\u216E\u216D\u216D",700, NumeralFlags.Unicode),
            new LiteralNumeral("\u216E\u216D\u216D\u216D",800, NumeralFlags.Unicode),
            new LiteralNumeral("\u216D\u216F",900, NumeralFlags.Unicode),

            new LiteralNumeral("\u216F",1000, NumeralFlags.Unicode),
            new LiteralNumeral("\u216F\u216F",2000, NumeralFlags.Unicode),
            new LiteralNumeral("\u216F\u216F\u216F",3000, NumeralFlags.Unicode),
            new LiteralNumeral("\u216F\u216F\u216F\u216F",4000, NumeralFlags.Unicode),
        };

        private static IList<LiteralNumeral> _literalNumerals;

        private static IList<LiteralNumeral> LiteralNumerals
        {
            get
            {
                if (_literalNumerals is null)
                {
                    _literalNumerals = new List<LiteralNumeral>(BaseLiteralNumerals);
                    foreach (var baseLiteralNumeral in BaseLiteralNumerals.Where(b => b.Flags == NumeralFlags.Ascii || b.Flags == NumeralFlags.Unicode))
                    {
                        if (baseLiteralNumeral.Digit < 1000)
                        {
                            _literalNumerals.Add(CreateVinculum(baseLiteralNumeral, '\u0305', 1000));
                            _literalNumerals.Add(CreateVinculum(baseLiteralNumeral, '\u033F', 1000000));
                        }
                    }
                }

                return _literalNumerals;
            }
        }

        private static LiteralNumeral CreateVinculum(LiteralNumeral numeral, char marker, int factor)
        {
            var literal = new StringBuilder();
            foreach (var c in numeral.Literal)
                literal.AppendFormat("{0}{1}", c, marker);
            return new LiteralNumeral(literal.ToString(), numeral.Digit * factor, numeral.Flags | NumeralFlags.Vinculum);
        }

        public static LiteralNumeral TryGet(uint digit, NumeralFlags flags)
        {
            return LiteralNumerals.FirstOrDefault(v => v.Digit == digit && v.Matches(flags));
        }
    }
}