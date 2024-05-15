using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CbPage_backend
{
    public class Program
    {
        public static void Main()
        {
            Server.HttpServer server = new Server.HttpServer();
            server.Start();
            while (true) { }

        }
    }
}