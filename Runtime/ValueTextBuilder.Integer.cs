namespace CodeWriter.ViewBinding
{
    public ref partial struct ValueTextBuilder
    {
        public void Append(long number)
        {
            if (number < 0)
            {
                Append('-');
                number = -number;
            }

            var start = _pos;

            do
            {
                Append((char) (number % 10 + '0'));
                number /= 10;
            } while (number > 0);

            var end = _pos - 1;

            while (start < end)
            {
                var t = _chars[start];
                _chars[start] = _chars[end];
                _chars[end] = t;

                start++;
                end--;
            }
        }
    }
}