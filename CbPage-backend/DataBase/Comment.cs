using System;
using System.Collections.Generic;
using System.Text;

namespace CbPage_backend.DataBase
{
    public class Comment : ITable
    {
        public string CommentText { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public Status Status { get; set; }

        public void PushToDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
