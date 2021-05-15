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
        private readonly IFilter filter;

        public TourPlannerClient(ITourApi api, IImportExportHandler handler, IFilter filter)
        {
            this.api = api;
            this.handler = handler;
            this.filter = filter;
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

        public bool FilterMethod(object o)
        {
            return filter.ApplyFilter(o);
        }

        public void UpdateFilter(string newFilter)
        {
            if (filter.Filter != newFilter)
                filter.Filter = newFilter;
        }
    }
}