using System.IO;
using WebService_Lib.Attributes;

namespace Server.DAL
{
    // TODO remove later
    [Component]
    public class MockMapApi : IMapApi
    {
        private readonly string path;
        public string RoutePath => path;
        public bool SaveRouteImage(string @from, string to, string id)
        {
            return true;
        }

        public string? GetRouteImagePath(string id)
        {
            return path + Path.DirectorySeparatorChar + "map.png";
        }

        public MockMapApi()
        {
            path = "." + Path.DirectorySeparatorChar + "maps";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}