

namespace Server.DAL
{
    /// <summary>
    /// Interface that describes methods to interact with a Map API.
    /// </summary>
    public interface IMapApi
    {
        string RoutePath { get; }
        
        /// <summary>
        /// Gets Route Information from the Map API.
        /// </summary>
        /// <param name="from">
        /// From (Source) location.
        /// </param>
        /// <param name="to">
        /// To (Destination) location.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with a MapApiResponse as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        (MapApiResponse?, string) GetRouteInfo(string from, string to);

        /// <summary>
        /// Gets the Route Information Image from the Map API and stores it locally on the filesystem.
        /// </summary>
        /// <param name="id">Tour id parameter.</param>
        /// <param name="boundingBox">BoundingBox parameter.</param>
        /// <param name="sessionId">SessionId parameter.</param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with a true as item1 and a empty string as item2 is returned.
        /// On failure item1 is false and item2 will contain the error message.
        /// </returns>
        (bool, string) SaveRouteImage(int id, string boundingBox, string sessionId);

        /// <summary>
        /// Returns the Route Image of an existing Tour from the filesystem.
        /// </summary>
        /// <param name="id">
        /// Id of the Tour of whom the Route Image is requested.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the path as item1 and null as item2 is returned.
        /// On failure item1 is false and item2 will contain the error message.
        /// </returns>
        (string?, string) GetRouteImagePath(int id);
    }
}