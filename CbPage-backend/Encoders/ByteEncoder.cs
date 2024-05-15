using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CbPage_backend.DataBase;

namespace CbPage_backend.Encoders
{
    public class ByteEncoder
    {
        public static byte[] Encode(byte[] input)
        {
            byte[] length = BitConverter.GetBytes(input.Length);
            byte[] bytes = new byte[input.Length + 2];
            bytes[0] = length[0];
            bytes[1] = length[1];
            Array.Copy(input, 0, bytes, 2, input.Length);
            return bytes;
        }

        public static byte[] Encode(string input)
        {
            byte[] stringu8 = Encoding.UTF8.GetBytes(input);
            byte[] bytes = new byte[stringu8.Length + 2];
            byte[] length = BitConverter.GetBytes(stringu8.Length);
            bytes[0] = length[0];
            bytes[1] = length[1];
            Array.Copy(stringu8, 0, bytes, 2, stringu8.Length);
            return bytes;
        }

        public static byte[] Encode(long input) => BitConverter.GetBytes(input);

        public static byte[] Encode(int input) => BitConverter.GetBytes(input);

        public static byte[] Encode(List<string> input)
        {
            List<byte[]> bytesList = new List<byte[]>(input.Capacity);

            foreach (string item in input)
                bytesList.Add(Encode(item));

            byte[] bytesData = (byte[])bytesList.SelectMany(x => x);
            byte[] bytes = new byte[bytesData.Length + 2];
            byte[] length = BitConverter.GetBytes(bytesData.Length);
            bytes[0] = length[0];
            bytes[1] = length[1];
            Array.Copy(bytesData, 0, bytes, 2, bytesData.Length);
            return bytes;
        }
    }
}
