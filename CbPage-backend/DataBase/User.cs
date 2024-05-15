using System;
using System.Collections.Generic;
using System.Text;

namespace CbPage_backend.DataBase
{
    public class User : ITable
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] salt { get; set; }
        public byte[] RealName { get; set; }
        public byte[] RealLastName { get; set; }
        public byte[] Email { get; set; }

        public void PushToDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
