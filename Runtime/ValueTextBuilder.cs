using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    public ref partial struct ValueTextBuilder
    {
        public const int DefaultCapacity = 128;

        private char[] _charsArray;
        private Span<char> _chars;
        private int _pos;

        public ValueTextBuilder(int initialCapacity)
        {
            _charsArray = ArrayPool<char>.Shared.Rent(initialCapacity);
            _chars = _charsArray;
            _pos = 0;
        }

        public int Length
        {
            get => _pos;
            set => _pos = value;
        }

        public int Capacity => _chars.Length;

        public void EnsureCapacity(int capacity)
        {
            if ((uint) capacity > (uint) _chars.Length)
            {
                Grow(capacity - _pos);
            }
        }

        public ref char this[int index] => ref _chars[index];

        public override string ToString()
        {
            var s = _chars.Slice(0, _pos).ToString();
            Dispose();
            return s;
        }

        public Span<char> RawChars => _chars;
        public char[] RawCharArray => _charsArray;

        public ReadOnlySpan<char> AsSpan(bool terminate)
        {
            if (terminate)
            {
                EnsureCapacity(Length + 1);
                _chars[Length] = '\0';
            }

            return _chars.Slice(0, _pos);
        }

        public ReadOnlySpan<char> AsSpan() => _chars.Slice(0, _pos);
        public ReadOnlySpan<char> AsSpan(int start) => _chars.Slice(start, _pos - start);
        public ReadOnlySpan<char> AsSpan(int start, int length) => _chars.Slice(start, length);

        public bool TryCopyTo(Span<char> destination, out int charsWritten)
        {
            if (_chars.Slice(0, _pos).TryCopyTo(destination))
            {
                charsWritten = _pos;
                Dispose();
                return true;
            }
            else
            {
                charsWritten = 0;
                Dispose();
                return false;
            }
        }

        public void Insert(int index, char value, int count)
        {
            if (_pos > _chars.Length - count)
            {
                Grow(count);
            }

            var remaining = _pos - index;
            _chars.Slice(index, remaining).CopyTo(_chars.Slice(index + count));
            _chars.Slice(index, count).Fill(value);
            _pos += count;
        }

        public void Insert(int index, string s)
        {
            if (s == null)
            {
                return;
            }

            var count = s.Length;

            if (_pos > (_chars.Length - count))
            {
                Grow(count);
            }

            var remaining = _pos - index;
            _chars.Slice(index, remaining).CopyTo(_chars.Slice(index + count));
            s.AsSpan().CopyTo(_chars.Slice(index));
            _pos += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char c)
        {
            var pos = _pos;
            if ((uint) pos < (uint) _chars.Length)
            {
                _chars[pos] = c;
                _pos = pos + 1;
            }
            else
            {
                GrowAndAppend(c);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string s)
        {
            if (s == null)
            {
                return;
            }

            var pos = _pos;
            if (s.Length == 1 && (uint) pos < (uint) _chars.Length)
            {
                _chars[pos] = s[0];
                _pos = pos + 1;
            }
            else
            {
                AppendSlow(s);
            }
        }

        private void AppendSlow(string s)
        {
            var pos = _pos;
            if (pos > _chars.Length - s.Length)
            {
                Grow(s.Length);
            }

            s.AsSpan().CopyTo(_chars.Slice(pos));
            _pos += s.Length;
        }

        public void Append(char c, int count)
        {
            if (_pos > _chars.Length - count)
            {
                Grow(count);
            }

            var dst = _chars.Slice(_pos, count);
            for (var i = 0; i < dst.Length; i++)
            {
                dst[i] = c;
            }

            _pos += count;
        }

        public void Append(string s, int start, int length)
        {
            var span = s.AsSpan(start, length);
            Append(span);
        }

        public void Append(ReadOnlySpan<char> value)
        {
            var pos = _pos;
            if (pos > _chars.Length - value.Length)
            {
                Grow(value.Length);
            }

            value.CopyTo(_chars.Slice(_pos));
            _pos += value.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<char> AppendSpan(int length)
        {
            var origPos = _pos;
            if (origPos > _chars.Length - length)
            {
                Grow(length);
            }

            _pos = origPos + length;
            return _chars.Slice(origPos, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowAndAppend(char c)
        {
            Grow(1);
            Append(c);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Grow(int additionalCapacityBeyondPos)
        {
            Debug.Assert(additionalCapacityBeyondPos > 0);
            Debug.Assert(_pos > _chars.Length - additionalCapacityBeyondPos,
                "Grow called incorrectly, no resize is needed.");

            var poolArray = ArrayPool<char>.Shared.Rent(
                (int) Math.Max((uint) (_pos + additionalCapacityBeyondPos),
                    (uint) _chars.Length * 2));

            _chars.Slice(0, _pos).CopyTo(poolArray);

            var toReturn = _charsArray;
            _chars = _charsArray = poolArray;
            if (toReturn != null)
            {
                ArrayPool<char>.Shared.Return(toReturn);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            var toReturn = _charsArray;
            this = default;
            if (toReturn != null)
            {
                ArrayPool<char>.Shared.Return(toReturn);
            }
        }
    }
}