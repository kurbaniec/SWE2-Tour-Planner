

namespace Server.DAL
{
    public interface IMapApi
    {
        string RoutePath { get; }
        
        (int?, string) SaveRouteImageAndReturnDistance(string from, string to, string id);

        string? GetRouteImagePath(int id);
    }
}