namespace Server.DAL
{
    public class MapApiResponse
    {
        public int Distance { get; }
        public string BoundingBox { get; }
        public string SessionId { get; }
        
        public MapApiResponse(int distance, string boundingBox, string sessionId)
        {
            Distance = distance;
            BoundingBox = boundingBox;
            SessionId = sessionId;
        }
    }
}