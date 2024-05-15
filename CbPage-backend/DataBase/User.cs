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
        public string RealName { get; set; }
        public string RealLastName { get; set; }
        public string Email { get; set; }

        public void PushToDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
