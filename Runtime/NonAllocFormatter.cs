using System.Text;

namespace CodeWriter.ViewBinding
{
    public static class NonAllocFormatter
    {
        public static void AppendInvariant(StringBuilder builder, int value)
        {
            if (value < 0)
            {
                builder.Append('-');
                // ReSharper disable once IntVariableOverflowInUncheckedContext
                AppendInvariant(builder, uint.MaxValue - ((uint) value) + 1);
            }
            else
            {
                AppendInvariant(builder, (uint) value);
            }
        }

        public static void AppendInvariant(this StringBuilder builder, uint value)
        {
            if (value == 0)
            {
                builder.Append('0');
                return;
            }

            var length = UintLength(value);
            builder.Append('0', length);
            length = builder.Length;

            do
            {
                var tmpValue = value;
                value /= 10;
                builder[--length] = (char) ('0' + (tmpValue - value * 10));
            } while (value > 0);
        }

        private static int UintLength(uint i)
        {
            if (i < 100000)
            {
                if (i < 10) return 1;
                if (i < 100) return 2;
                if (i < 1000) return 3;
                if (i < 10000) return 4;
                return 5;
            }
            else
            {
                if (i < 1000000) return 6;
                if (i < 10000000) return 7;
                if (i < 100000000) return 8;
                if (i < 1000000000) return 9;
                return 10;
            }
        }
    }
}