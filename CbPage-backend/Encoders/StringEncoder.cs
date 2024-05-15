using System;
using System.Collections.Generic;
using System.Text;

namespace CbPage_backend.Encoders
{
    internal class StringEncoder
    {
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

        public static string Decode(byte[] input)
        {
            int lenth = BitConverter.ToInt16(input, 0);
            byte[] stringu8 = new byte[lenth];
            Array.Copy(input, 2, stringu8, 0, lenth);
            return Encoding.UTF8.GetString(stringu8);
        }
    }
}
