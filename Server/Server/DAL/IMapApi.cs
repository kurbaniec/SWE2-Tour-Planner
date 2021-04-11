

namespace Server.DAL
{
    public interface IMapApi
    {
        string RoutePath { get; }
        
        bool SaveRouteImage(string from, string to, string id);

        string? GetRouteImagePath(int id);
    }
}