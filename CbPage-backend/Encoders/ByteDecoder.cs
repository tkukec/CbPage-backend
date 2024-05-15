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
            int length = BitConverter.ToInt16(bytesRaw);

            byte[] bytes = new byte[length];
            Array.Copy(bytesRaw, 2, bytes, 0, length);
            return (bytes, length + 2);
        }

        public static (string, int) DecodeString(Span<byte> input)
        {
            byte[] bytesRaw = input.ToArray();
            int length = BitConverter.ToInt16(bytesRaw);

            byte[] stringu8 = new byte[length];
            Array.Copy(bytesRaw, 2, stringu8, 0, length);
            return (Encoding.UTF8.GetString(stringu8), length + 2);
        }

        public static (int, int) DecodeInt(Span<byte> input)
            => (BitConverter.ToInt32(input), sizeof(int));

        public static (long, int) DecodeLong(Span<byte> input)
            => (BitConverter.ToInt64(input), sizeof(long));

        public static (List<string>, int) DecodeStringList(Span<byte> input)
        {
            byte[] bytesRaw = input.ToArray();
            int lenght = BitConverter.ToInt16(bytesRaw);

            Span<byte> bytesSpan = new Span<byte>(bytesRaw, 2, lenght);
            List<string> strings = new List<string>();
            int lenghtSoFar = 0;
            while (lenghtSoFar < lenght)
            {
                (string currentString, int currentLenght) = DecodeString(bytesSpan);
                strings.Add(currentString);
                lenghtSoFar += currentLenght;
                bytesSpan = new Span<byte>(bytesRaw, lenghtSoFar + 2, lenght - lenghtSoFar);
            }

            return (strings, lenght + 2);
        }
    }
}
