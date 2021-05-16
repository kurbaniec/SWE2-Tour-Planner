using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Client.Logic.DAL;
using Model;

namespace Client.Logic.BL
{
    /// <summary>
    /// Business Layer of the Client WPF application that provides access to core functionality.
    /// </summary>
    public class TourPlannerClient
    {
        private readonly ITourApi api;
        private readonly IImportExportHandler handler;
        private readonly IFilter filter;

        public TourPlannerClient(ITourApi api, IImportExportHandler handler, IFilter filter)
        {
            this.api = api;
            this.handler = handler;
            this.filter = filter;
        }
        
        /// <summary>
        /// Query all Tours from the configured Data Access Layer (DAL).
        /// </summary>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the queried Tours as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        public async Task<(List<Tour>?, string)> GetTours()
        {
            return await api.GetTours();
        }
        
        /// <summary>
        /// Adds a new Tour in the configured DAL.
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
            return await api.AddTour(tour);
        }
        
        /// <summary>
        /// Adds multiple new Tours in the configured DAL.
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
            return await api.AddTours(tours);
        }
        
        /// <summary>
        /// Updates an existing Tour in the configured DAL.
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
            return await api.UpdateTour(tour);
        }

        /// <summary>
        /// Deletes an existing Tour in the configured DAL.
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
            return await api.DeleteTour(id);
        }

        /// <summary>
        /// Requests the Route Image of an existing Tour in the configured DAL.
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
            return await api.GetRouteImage(id);
        }

        /// <summary>
        /// Deserializes Tours from earlier serialized, exported Tour data.
        /// </summary>
        /// <param name="inputPath">
        /// Path to the file with the exported Tour data.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the deserialized Tours as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        public async Task<(List<Tour>?, string)> ImportTours(string inputPath)
        {
            return await handler.ImportTours(inputPath);
        }

        /// <summary>
        /// Serializes and exports given Tours.
        /// </summary>
        /// <param name="outputPath">
        /// Path to the file where the Tour data should be exported.
        /// </param>
        /// <param name="tours">
        /// Tours that should be exported.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with true as item1 and a empty string as item2 is returned.
        /// On failure item1 is false and item2 will contain the error message.
        /// </returns>
        public async Task<(bool, string)> ExportTours(string outputPath, List<Tour> tours)
        {
            return await handler.ExportTours(outputPath, tours);
        }

        /// <summary>
        /// Exports a printable document from a given Tour.
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
        public async Task<(bool, string)> Print(string outputPath, int id, bool isSummary = false)
        {
            return await api.GetExport(id, outputPath, isSummary);
        }

        /// <summary>
        /// Method that can be used to filter objects in an <c>CollectionView</c>.
        /// </summary>
        /// <param name="o">
        /// Object that will be checked with the filter criteria.
        /// </param>
        /// <returns>
        /// True, if the object should be displayed, else false.
        /// </returns>
        public bool FilterMethod(object o)
        {
            return filter.ApplyFilter(o);
        }

        /// <summary>
        /// Update the filter string used to determine objects to be filtered in <c>FilterMethod</c>.
        /// </summary>
        /// <param name="newFilter">
        /// The new value for the filter.
        /// </param>
        public void UpdateFilter(string newFilter)
        {
            if (filter.Filter != newFilter)
                filter.Filter = newFilter;
        }
    }
}