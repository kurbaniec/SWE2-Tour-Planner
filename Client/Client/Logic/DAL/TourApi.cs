using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Logic.Setup;
using Model;

namespace Client.Logic.DAL
{
    public class TourApi : ITourApi
    {
        private readonly Configuration cfg;
        private readonly string baseUrl;
        private readonly HttpClient client = new();

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