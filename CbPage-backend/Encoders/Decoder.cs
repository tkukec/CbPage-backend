using System;
using System.Collections.Generic;
using System.Text;

namespace CbPage_backend.Encoders
{
    internal class Decoder
    {
        public static byte[] DecodeArray(byte[] input)
        {
            int lenth = BitConverter.ToInt16(input, 0);

            byte[] bytes = new byte[lenth];
            Array.Copy(input, 2, bytes, 0, lenth);
            return bytes;
            
        }

        public static string DecodeString(byte[] input)
        {
            int lenth = BitConverter.ToInt16(input, 0);
            byte[] stringu8 = new byte[lenth];
            Array.Copy(input, 2, stringu8, 0, lenth);
            return Encoding.UTF8.GetString(stringu8);
        }
    }
}
