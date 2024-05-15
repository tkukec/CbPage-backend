using System;
using System.Collections.Generic;
using System.Text;

namespace CbPage_backend.Encoders
{
    internal class StringEncoder
    {
        public static byte[] Encode(string input)
        {
            return Encoding.UTF8.GetBytes(input);
        }

        public static string Decode(byte[] input)
        {
            return Encoding.UTF8.GetString(input);
        }
    }
}
