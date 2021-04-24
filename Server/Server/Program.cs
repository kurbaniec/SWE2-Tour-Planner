using System;
using WebService_Lib;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // Port is set through config file 
            // with the help of `Setup/Configuration.cs`
            var service = new SimpleWebService();
            service.Start();
        }
    }
}
