

namespace Server.DAL
{
    public interface IMapApi
    {
        string RoutePath { get; }
        
        (MapApiResponse?, string) GetRouteInfo(string from, string to);

        (bool, string) SaveRouteImage(string id, string boundingBox, string sessionId);

        string? GetRouteImagePath(int id);
    }
}