﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Server.Setup;
using WebService_Lib.Attributes;
using WebService_Lib.Logging;

namespace Server.DAL
{
    /// <summary>
    /// Concrete implementation of <c>IMapApi</c>.
    /// MapQuest is used for the Map API.
    /// </summary>
    [Component]
    public class MapQuestApi : IMapApi
    {
        [Autowired] private readonly Configuration cfg = null!;
        private readonly HttpClient client = new();
        private readonly ILogger logger = WebServiceLogging.CreateLogger<IMapApi>();
        private string ApiKey => cfg.MapApiKey;
        public string RoutePath => cfg.RoutePath;

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
                    JObject mapQuestInfo = JObject.Parse(jsonString);
                    if (mapQuestInfo["route"]?["distance"] is { } distanceRaw &&
                        mapQuestInfo["route"]?["boundingBox"] is { } boundingBoxRaw &&
                        mapQuestInfo["route"]?["sessionId"] is { } sessionIdRaw)
                    {
                        var distance = (double) distanceRaw;
                        var boundingBox =
                            boundingBoxRaw["ul"]!["lat"]!.ToString().Replace(",", ".") + ","
                            + boundingBoxRaw["ul"]!["lng"]!.ToString().Replace(",", ".") + ","
                            + boundingBoxRaw["lr"]!["lat"]!.ToString().Replace(",", ".") + ","
                            + boundingBoxRaw["lr"]!["lng"]!.ToString().Replace(",", ".");
                        var sessionId = (string) sessionIdRaw!;
                        logger.Log(LogLevel.Information, "Received MapQuest Tour Info successfully");
                        return (new MapApiResponse(
                            distance, boundingBox, sessionId
                        ), string.Empty);
                    }

                    logger.Log(LogLevel.Error, 
                        "MapQuest did not deliver expected results probably because invalid given route");
                    return (null, "Invalid route given. Please try again.");
                }

                logger.Log(LogLevel.Error, "MapQuest Tour Info request was not successful");
                return (null, "Encountered problems while trying to reach MapQuest API.");
            }
            catch (JsonException jsonEx)
            {
                logger.Log(LogLevel.Error, jsonEx.StackTrace);
                return (null, "Encountered problems while parsing MapQuest response.");
            }
            catch (HttpRequestException httpEx)
            {
                logger.Log(LogLevel.Error, httpEx.StackTrace);
                return (null, "Encountered problems while trying to reach MapQuest API.");
            }
        }

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
        public (bool, string) SaveRouteImage(int id, string boundingBox, string sessionId)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string?, string?>()
            {
                {"key", ApiKey}, {"session", sessionId}, {"boundingBox", boundingBox},
                {"size", "1920,1440"}, {"format", "png"}
            });
            string query = content.ReadAsStringAsync().Result;
            try
            {
                var response = client.GetAsync(
                    $"http://www.mapquestapi.com/staticmap/v5/map?{query}").Result;

                if (response.IsSuccessStatusCode)
                {
                    using (var fs = new FileStream(
                        $"{RoutePath}{Path.DirectorySeparatorChar}{id}.png",
                        FileMode.Create))
                    {
                        response.Content.CopyToAsync(fs).Wait();
                    }
                    logger.Log(LogLevel.Information, "Received Route information image successfully");
                    return (true, string.Empty);
                }

                logger.Log(LogLevel.Error, "MapQuest Staticmap request was not successful");
                return (false, "Encountered problems while trying to reach MapQuest API.");
            }
            catch (JsonException jsonEx)
            {
                logger.Log(LogLevel.Error, jsonEx.StackTrace);
                return (false, "Encountered problems while parsing MapQuest response.");
            }
            catch (HttpRequestException httpEx)
            {
                logger.Log(LogLevel.Error, httpEx.StackTrace);
                return (false, "Encountered problems while trying to reach MapQuest API.");
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.StackTrace);
                return (false, "Encountered problems while downloading image.");
            }
        }

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
        public (string?, string) GetRouteImagePath(int id)
        {
            var path = $"{RoutePath}{Path.DirectorySeparatorChar}{id}.png";
            if (File.Exists(path))
                return (path, string.Empty);
            logger.Log(LogLevel.Error, $"File {path} does not exists");
            return (null, "File does not exist");
        }
    }
}