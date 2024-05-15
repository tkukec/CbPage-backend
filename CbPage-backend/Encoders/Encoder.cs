using System;
using System.Collections.Generic;
using System.Text;

namespace CbPage_backend.Encoders
{
    internal class Encoder
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
    }
}
