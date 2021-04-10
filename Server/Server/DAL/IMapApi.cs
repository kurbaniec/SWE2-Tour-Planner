

namespace Server.DAL
{
    public interface IMapApi
    {
        string RoutePath { get; }
        
        bool SaveRouteImage(string from, string to, string url, string id);

        File? GetRouteImage(string id);
    }
}