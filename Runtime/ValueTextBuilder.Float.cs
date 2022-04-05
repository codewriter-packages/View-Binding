namespace CodeWriter.ViewBinding
{
    public ref partial struct ValueTextBuilder
    {
        public void Append(float value, int precision = 9, bool fixedPrecision = false)
        {
            if (value < 0)
            {
                Append('-');
                value = -value;
            }

            var valueD = (decimal) value;
            var integer = (long) valueD;

            Append(integer);

            if (precision > 0)
            {
                valueD -= integer;

                if (valueD != 0)
                {
                    Append('.');

                    for (var p = 0; p < precision; p++)
                    {
                        valueD *= 10;
                        var d = (long) valueD;

                        Append((char) (d + '0'));
                        valueD -= d;

                        if (valueD == 0 && !fixedPrecision)
                        {
                            p = precision;
                        }
                    }
                }
            }
        }
    }
}