using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Client.Logic.Setup;
using Client.Utils.Logging;
using Microsoft.Extensions.Logging;
using Model;
using Newtonsoft.Json;

namespace Client.Logic.DAL
{
    /// <summary>
    /// Concrete implementation of <c>ITourApi</c>.
    /// Communicates with TourPlanner service to invoke CRUD operations.
    /// </summary>
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

        /// <summary>
        /// Query all Tours from the service.
        /// </summary>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the queried Tours as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
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

        /// <summary>
        /// Adds a new Tour in the service.
        /// </summary>
        /// <param name="tour">
        /// Tour which should be added.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the added Tour as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
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

        /// <summary>
        /// Adds multiple new Tours in the service.
        /// </summary>
        /// <param name="tours">
        /// Tours which should be added.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the added Tours as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        public async Task<(List<Tour>?, string)> AddTours(List<Tour> tours)
        {
            try
            {
                var response = await client.PostAsync($"{baseUrl}/api/tours",
                    new StringContent(
                        JsonConvert.SerializeObject(tours),
                        Encoding.Default,
                        "application/json"
                    ));
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var newTours = JsonConvert.DeserializeObject<List<Tour>>(jsonString);
                    logger.Log(LogLevel.Information, "Received new tours successfully");
                    return (newTours, string.Empty);
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

        /// <summary>
        /// Updates an existing Tour in the service.
        /// </summary>
        /// <param name="tour">
        /// Tour which should be updated.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the updated Tour as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        public async Task<(Tour?, string)> UpdateTour(Tour tour)
        {
            try
            {
                var response = await client.PutAsync($"{baseUrl}/api/tour",
                    new StringContent(
                        JsonConvert.SerializeObject(tour),
                        Encoding.Default,
                        "application/json"
                    ));
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var newTour = JsonConvert.DeserializeObject<Tour>(jsonString);
                    logger.Log(LogLevel.Information, $"Tour with id {tour.Id} updated successfully");
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

        /// <summary>
        /// Deletes an existing Tour in the service.
        /// </summary>
        /// <param name="id">
        /// The id of the Tour which should be deleted.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the true as item1 and a empty string as item2 is returned.
        /// On failure item1 is false and item2 will contain the error message.
        /// </returns>
        public async Task<(bool, string)> DeleteTour(int id)
        {
            try
            {
                var response = await client.DeleteAsync($"{baseUrl}/api/tour/{id}");
                if (response.IsSuccessStatusCode)
                {
                    logger.Log(LogLevel.Information, $"Tour with id {id} deleted successfully");
                    return (true, string.Empty);
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                logger.Log(LogLevel.Error, errorMsg);
                return (false, errorMsg);
            }
            catch (HttpRequestException httpEx)
            {
                logger.Log(LogLevel.Error, httpEx.StackTrace);
                return (false, httpEx.Message);
            }
        }

        /// <summary>
        /// Requests the Route Image of an existing Tour from the service.
        /// Placeholder image is generated locally when problems occur.
        /// </summary>
        /// <param name="id">
        /// Id of the Tour of whom the Route Image is requested.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the Route Image as item1 and null as item2 is returned.
        /// On failure item1 is null and item2 will contain a placeholder image.
        /// </returns>
        public async Task<(BitmapImage?, BitmapImage?)> GetRouteImage(int id)
        {
            try
            {
                // Get route image from server
                using var response = await client.GetAsync($"{baseUrl}/api/route/{id}");
                if (response.IsSuccessStatusCode)
                {
                    // Get BitmapImage
                    // See: https://stackoverflow.com/a/46709476/12347616
                    await using var stream = new MemoryStream();
                    await response.Content.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    var routeImage = new BitmapImage();
                    routeImage.BeginInit();
                    routeImage.CacheOption = BitmapCacheOption.OnLoad;
                    routeImage.StreamSource = stream;
                    routeImage.EndInit();
                    routeImage.Freeze();
                    logger.Log(LogLevel.Information,
                        $"Received Route Information Image for Tour with id {id} successfully");
                    return (routeImage, null);
                }
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.StackTrace);
            }

            logger.Log(LogLevel.Warning,
                $"Route Information Image for Tour with id {id} not received successfully");
            // Create Dummy Route Image
            // ---
            // Draw rectangle
            // See: https://stackoverflow.com/a/1720261/12347616
            var bitmap = new Bitmap(1920, 1440);
            using Graphics gfx = Graphics.FromImage(bitmap);
            gfx.FillRectangle(
                new SolidBrush(Color.Salmon), 0, 0, 1920, 1440);
            gfx.DrawString(
                "Could not load image.\nPlease try again later.",
                new Font("Arial", 24),
                new SolidBrush(Color.Black), 120, 120);
            // Convert to BitMapImage
            // See: https://stackoverflow.com/a/23831231/12347616
            await using var memory = new MemoryStream();
            bitmap.Save(memory, ImageFormat.Png);
            memory.Position = 0;
            var dummyRouteImage = new BitmapImage();
            dummyRouteImage.BeginInit();
            dummyRouteImage.StreamSource = memory;
            dummyRouteImage.CacheOption = BitmapCacheOption.OnLoad;
            dummyRouteImage.EndInit();
            dummyRouteImage.Freeze();
            logger.Log(LogLevel.Warning, "Dummy Route Image will be used");
            return (null, dummyRouteImage);
        }

        /// <summary>
        /// Requests a printable document from a given Tour from the service.
        /// </summary>
        /// <param name="outputPath">
        /// Path to the file where the document should be exported.
        /// </param>
        /// <param name="id">
        /// Id of the Tour that should be printed.
        /// </param>
        /// <param name="isSummary">
        /// Determines if a summary or full report is generated.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with true as item1 and a empty string as item2 is returned.
        /// On failure item1 is false and item2 will contain the error message.
        /// </returns>
        public async Task<(bool, string)> GetExport(int id, string outputPath, bool isSummary = false)
        {
            try
            {
                // Get pdf export from server
                string url = !isSummary ? $"{baseUrl}/api/export/full/{id}" : $"{baseUrl}/api/export/summary/{id}";
                using var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    await using var stream = File.Create(outputPath);
                    await response.Content.CopyToAsync(stream);
                    return (true, string.Empty);
                }

                var errorMsg = await response.Content.ReadAsStringAsync();
                logger.Log(LogLevel.Error, errorMsg);
                return (false, errorMsg);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, ex.StackTrace);
                return (false, ex.Message);
            }
        }
    }
}