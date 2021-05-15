using System;
using System.IO;
using Server.DAL;

namespace Server_Test.Unit.Mocks
{
    public class MockMapApi : IMapApi
    {
        private readonly string path;
        public string RoutePath => path;
        public (MapApiResponse?, string) GetRouteInfo(string@from, string to)
        {
            return (new MapApiResponse(1, "", ""), string.Empty);
        }

        public (bool, string) SaveRouteImage(int id, string boundingBox, string sessionId)
        {
            return (true, "");
        }

        public (string?, string) GetRouteImagePath(int id)
        {
            //return (path + Path.DirectorySeparatorChar + "map.png", string.Empty);
            return (null, string.Empty);
        }

        public MockMapApi()
        {
            /*
            // Get project directory
            string runningPath = AppDomain.CurrentDomain.BaseDirectory!;
            // Platform agnostic path
            // See: https://stackoverflow.com/a/38428899/12347616
            path =
                $"{Path.GetFullPath(Path.Combine(runningPath!, @$"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}"))}maps";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }*/
        }
    }
}