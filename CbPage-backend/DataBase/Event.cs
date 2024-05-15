using System;
using System.Collections.Generic;
using System.Text;

namespace CbPage_backend.DataBase
{
    public class Event : ITable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string AffectedBrand { get; set; }
        public string URL { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Status status { get; set; }
        public List<string> DNSRecords { get; set; }
        public List<string> Keywords { get; set; }
        public List<string> Comments { get; set; }

        public void PushToDatabase()
        {
            throw new NotImplementedException();
        }
    }
    
}
