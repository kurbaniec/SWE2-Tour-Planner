using System;
using System.IO;
using Server.Setup;
using WebService_Lib.Attributes;

namespace Server.DAL
{
    // TODO remove later
    [Component]
    public class MapQuestApi : IMapApi
    {
        [Autowired]
        private readonly Configuration cfg = null!;
        private readonly string path;
        public string RoutePath => path;

        public (int?, string) SaveRouteImageAndReturnDistance(string @from, string to, string id)
        {
            return (1, string.Empty);
        }

        public string? GetRouteImagePath(int id)
        {
            return path + Path.DirectorySeparatorChar + "map.png";
        }

        public MapQuestApi()
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