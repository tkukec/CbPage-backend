using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CbPage_backend.DataBase
{
    public class User : ITable
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] salt { get; set; }
        public string RealName { get; set; }
        public string RealLastName { get; set; }
        public string Email { get; set; }

        public void PushToDatabase()
        {
            throw new NotImplementedException();
        }

        void GenerateSalt()
        {
            byte[][] bytesArray =
            {
                Encoding.UTF8.GetBytes(Username),
                Encoding.UTF8.GetBytes(RealName),
                Encoding.UTF8.GetBytes(RealLastName),
                Encoding.UTF8.GetBytes(Email),
                BitConverter.GetBytes(DateTime.Now.Ticks),
                new byte[128]
            };
            new Random().NextBytes(bytesArray.Last());
            byte[] bytes = (byte[])bytesArray.SelectMany(x => x);
            using (SHA512 sha512 = SHA512.Create())
                bytes = sha512.ComputeHash(bytes);
            salt = bytes;
        }
    }
}
