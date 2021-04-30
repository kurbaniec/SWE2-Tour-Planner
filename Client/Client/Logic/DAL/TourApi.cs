using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Client.Logic.Setup;
using Client.Utils.Logging;
using Microsoft.Extensions.Logging;
using Model;
using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;

namespace Client.Logic.DAL
{
    public class TourApi : ITourApi
    {
        private readonly Configuration cfg;
        private readonly string baseUrl;
        private readonly HttpClient client = new();
        private readonly ILogger logger = ApplicationLogging.CreateLogger<ITourApi>();

        public TourApi(Configuration cfg)
        {
            this.cfg = cfg;
            this.baseUrl = cfg.BaseUrl;
        }

        public async Task<(List<Tour>?, string)> GetTours()
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/api/tours");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var tours = JsonConvert.DeserializeObject<List<Tour>>(jsonString);
                    logger.Log(LogLevel.Information, "Received tours successfully");
                    return (tours, string.Empty);
                }
            }
            catch (JsonException jsonEx)
            {
                logger.Log(LogLevel.Error, jsonEx.StackTrace);
                return (null, jsonEx.Message);
            }
            catch (HttpRequestException httpEx)
            {
                logger.Log(LogLevel.Error, httpEx.StackTrace);
                return (null, httpEx.Message);
            }
            logger.Log(LogLevel.Error, "Unknown Error");
            return (null, "Unknown Error");
        }

        public async Task<(Tour?, string)> AddTour(Tour tour)
        {
            try
            {
                var response = await client.PostAsync($"{baseUrl}/api/tour",
                    new StringContent(
                        JsonConvert.SerializeObject(tour),
                        Encoding.Default,
                        "application/json"
                    ));
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var newTour = JsonConvert.DeserializeObject<Tour>(jsonString);
                    logger.Log(LogLevel.Information, "Received new tour successfully");
                    return (newTour, string.Empty);
                }
                var errorMsg = await response.Content.ReadAsStringAsync();
                logger.Log(LogLevel.Error, errorMsg);
                return (null, errorMsg);
            }
            catch (JsonException jsonEx)
            {
                logger.Log(LogLevel.Error, jsonEx.StackTrace);
                return (null, jsonEx.Message);
            }
            catch (HttpRequestException httpEx)
            {
                logger.Log(LogLevel.Error, httpEx.StackTrace);
                return (null, httpEx.Message);
            }
        }

        public async Task<(Tour?, string)> UpdateTour(Tour tour)
        {
            throw new System.NotImplementedException();
        }

        public async Task<(bool, string)> DeleteTour(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}