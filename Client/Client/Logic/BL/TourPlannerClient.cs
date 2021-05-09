using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Client.Logic.DAL;
using Model;

namespace Client.Logic.BL
{
    public class TourPlannerClient
    {
        private readonly ITourApi api;
        private readonly IImportExportHandler handler;

        public TourPlannerClient(ITourApi api, IImportExportHandler handler)
        {
            this.api = api;
            this.handler = handler;
        }
        
        public async Task<(List<Tour>?, string)> GetTours()
        {
            return await api.GetTours();
        }
        
        public async Task<(Tour?, string)> AddTour(Tour tour)
        {
            return await api.AddTour(tour);
        }
        
        public async Task<(List<Tour>?, string)> AddTours(List<Tour> tours)
        {
            return await api.AddTours(tours);
        }
        
        public async Task<(Tour?, string)> UpdateTour(Tour tour)
        {
            return await api.UpdateTour(tour);
        }

        public async Task<(bool, string)> DeleteTour(int id)
        {
            return await api.DeleteTour(id);
        }

        public async Task<(BitmapImage?, BitmapImage?)> GetRouteImage(int id)
        {
            return await api.GetRouteImage(id);
        }

        public async Task<(List<Tour>?, string)> ImportTours(string inputPath)
        {
            return await handler.ImportTours(inputPath);
        }

        public async Task<(bool, string)> ExportTours(string outputPath, List<Tour> tours)
        {
            return await handler.ExportTours(outputPath, tours);
        }

        public async Task<(bool, string)> Print(string outputPath, int id, bool isSummary = false)
        {
            return await api.GetExport(id, outputPath, isSummary);
        }
    }
}