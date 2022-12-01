namespace Common
{
    public static class ByteExtensions
    {
        public static bool IsBitSet(this byte b, int offset)
        {
            return (b & (1 << offset)) != 0;
        }

        public static bool IsBitNotSet(this byte b, int offset)
        {
            return (b & (1 << offset)) == 0;
        }

    
        public static int ExtractNumber(this byte b, int start, int length)
        {
            var shiftM = 7 - length + 1;
            var shiftB = 7 - length - start + 1;    
            var mask = (byte)(byte.MaxValue >> shiftM);

            return (b >> shiftB) & mask;
        }

        public static int ExtractNumber(this byte b, byte nextByte, int start, int length)
        {
            var shiftM = 15 - length + 1;
            var shiftB = Math.Max(15 - length - start + 1, 0);    
            var mask = (short)(65535 >> shiftM);

            var x = (b << 8) + nextByte;
            return (x >> shiftB) & mask;
        }

        public static int ExtractNumber(this byte[] bytes, int start, int length)
        {
            var index = start / 8;
            if (bytes.Length > index + 1)
            {
                var n = bytes[index].ExtractNumber(bytes[index + 1], start % 8, Math.Min(length, 16 - start % 8));
                if (start % 8 + length > 16)
                {
                    var extraLength = Math.Max(0, length - 16 + start % 8);
                    n <<= extraLength;
                    n += bytes[index + 2].ExtractNumber(0, extraLength);
                }

                return n;
            }

            return bytes[index].ExtractNumber(start % 8, length);
        }

        public static int Next(this byte[] bytes, ref int position, int length)
        {
            var result = bytes.ExtractNumber(position, length);

            position += length;

            return result;
        }
    }
}