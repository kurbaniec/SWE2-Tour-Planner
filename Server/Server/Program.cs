using System;
using WebService_Lib;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new SimpleWebService(port: 8080);
            service.Start();
        }
    }
}
