using System;
using System.Collections.Generic;
using System.Text;

namespace CbPage_backend.Encoders
{
    internal class ByteDecoder
    {
        public static (byte[], int) DecodeArray(Span<byte> input)
        {
            byte[] bytesRaw = input.ToArray();
            int length = BitConverter.ToInt16(bytesRaw, 0);

            byte[] bytes = new byte[length];
            Array.Copy(bytesRaw, 2, bytes, 0, length);
            return (bytes, length + 2);
        }

        public static (string, int) DecodeString(Span<byte> input)
        {
            byte[] bytesRaw = input.ToArray();
            int length = BitConverter.ToInt16(bytesRaw, 0);

            byte[] stringu8 = new byte[length];
            Array.Copy(bytesRaw, 2, stringu8, 0, length);
            return (Encoding.UTF8.GetString(stringu8), length + 2);
        }
    }
}
