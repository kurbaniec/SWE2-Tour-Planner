﻿

namespace Server.DAL
{
    public interface IMapApi
    {
        string RoutePath { get; }
        
        (MapApiResponse?, string) GetRouteInfo(string from, string to);

        (bool, string) SaveRouteImage(int id, string boundingBox, string sessionId);

        (string?, string) GetRouteImagePath(int id);
    }
}