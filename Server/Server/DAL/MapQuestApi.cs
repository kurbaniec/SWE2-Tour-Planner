using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Server.Setup;
using WebService_Lib.Attributes;
using WebService_Lib.Logging;

namespace Server.DAL
{
    [Component]
    public class MapQuestApi : IMapApi
    {
        [Autowired] 
        private readonly Configuration cfg = null!;
        private readonly HttpClient client = new();
        private readonly ILogger logger = WebServiceLogging.CreateLogger<IMapApi>();
        private string ApiKey => cfg.MapApiKey;
        public string RoutePath => cfg.RoutePath;

        public (MapApiResponse?, string) GetRouteInfo(string from, string to)
        {
            // Make request with parameters
            // See: https://stackoverflow.com/a/56824109/12347616
            // And: https://stackoverflow.com/a/26744471/12347616
            var content = new FormUrlEncodedContent(new Dictionary<string?, string?>()
            {
                {"key", ApiKey}, {"from", from}, {"to", to}
            });
            string query = content.ReadAsStringAsync().Result;
            try
            {
                var response = client.GetAsync(
                    $"http://www.mapquestapi.com/directions/v2/route?{query}").Result;
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    // TODO parse response and return it
                    JObject mapQuestInfo = JObject.Parse(jsonString);
                    logger.Log(LogLevel.Information, "Received MapQuest Tour Info successfully");
                    return (new MapApiResponse(1, "", ""), string.Empty);
                }
                logger.Log(LogLevel.Error, "MapQuest Tour Info request was not successful");
                return (null, "Encountered problems while trying to reach MapQuest API");
            }
            catch (HttpRequestException httpEx)
            {
                logger.Log(LogLevel.Error, httpEx.StackTrace);
                return (null, "Encountered problems while trying to reach MapQuest API");
            }
        }

        public (bool, string) SaveRouteImage(string id, string boundingBox, string sessionId)
        {
            throw new NotImplementedException();
        }

        public string? GetRouteImagePath(int id)
        {
            return RoutePath + Path.DirectorySeparatorChar + "map.png";
        }
    }
}