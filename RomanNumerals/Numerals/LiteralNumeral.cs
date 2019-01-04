namespace RomanNumerals.Numerals
{
    public class LiteralNumeral
    {
        public int Digit { get; }
        public string Literal { get; }
        public NumeralFlags Flags { get; }

        public LiteralNumeral(string literal, int digit, NumeralFlags flags = 0)
        {
            Literal = literal;
            Digit = digit;
            Flags = flags;
            //Flags = AdjustFlags(literal, flags);
        }

        private static NumeralFlags AdjustFlags(string literal, NumeralFlags flags)
        {
            foreach (var c in literal)
            {
                if (c >= 0x80)
                {
                    flags |= NumeralFlags.Unicode;
                    break;
                }
            }

            return flags;
        }

        public bool Matches(NumeralFlags flags)
        {
            return (Flags & ~flags) == 0;
        }
    }
}
