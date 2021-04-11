using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Model;

namespace Client.Logic.DAL
{
    public class TourApi : ITourApi
    {
        // TODO read base_url from config
        private readonly string baseUrl = "http://localhost:8080";
        private readonly HttpClient client = new();
        
        public async Task<(List<Tour>?, string)> GetTours()
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/api/tours");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var tours = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Tour>>(jsonString);
                    return (tours, string.Empty);
                }
            }
            catch (JsonException jsonEx)
            {
                return (null, jsonEx.Message);
            }
            catch (HttpRequestException httpEx)
            {
                return (null, httpEx.Message);
            }

            return (null, "Unknown Error");
        }

        public async Task<(Tour?, string)> AddTour(Tour tour)
        {
            throw new System.NotImplementedException();
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