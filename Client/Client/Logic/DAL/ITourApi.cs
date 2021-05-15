using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Model;

namespace Client.Logic.DAL
{
    /// <summary>
    /// Interface that describes methods to interact with the TourPlanner service (Server).
    /// </summary>
    public interface ITourApi
    {
        /// <summary>
        /// Query all Tours from the service.
        /// </summary>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the queried Tours as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        Task<(List<Tour>?, string)> GetTours();

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
        Task<(Tour?, string)> AddTour(Tour tour);

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
        Task<(List<Tour>?, string)> AddTours(List<Tour> tours);

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
        Task<(Tour?, string)> UpdateTour(Tour tour);

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
        Task<(bool, string)> DeleteTour(int id);

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
        Task<(BitmapImage?, BitmapImage?)> GetRouteImage(int id);

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
        Task<(bool, string)> GetExport(int id, string outputPath, bool isSummary = false);
    }
}