using WebService_Lib;

namespace Server
{
    /// <summary>
    /// Entry point for the TourPlanner Server application.
    /// Powered by <c>WebServiceLib</c>.
    /// </summary>
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
