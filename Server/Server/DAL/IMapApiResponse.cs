namespace Server.DAL
{
    public class MapApiResponse
    {
        public double Distance { get; }
        public string BoundingBox { get; }
        public string SessionId { get; }
        
        public MapApiResponse(double distance, string boundingBox, string sessionId)
        {
            Distance = distance;
            BoundingBox = boundingBox;
            SessionId = sessionId;
        }
    }
}