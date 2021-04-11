using System;
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

        public string? GetRouteImagePath(int id)
        {
            return path + Path.DirectorySeparatorChar + "map.png";
        }

        public MockMapApi()
        {
            // Get project directory
            string runningPath = AppDomain.CurrentDomain.BaseDirectory!;
            // Platform agnostic path
            // See: https://stackoverflow.com/a/38428899/12347616
            path =
                $"{Path.GetFullPath(Path.Combine(runningPath!, @$"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}"))}maps";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}