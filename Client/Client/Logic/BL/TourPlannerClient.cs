using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Client.Logic.DAL;
using Model;

namespace Client.Logic.BL
{
    public class TourPlannerClient
    {
        private ITourApi api;

        public TourPlannerClient(ITourApi api)
        {
            this.api = api;
        }
        
        public async Task<(List<Tour>?, string)> GetTours()
        {
            return await api.GetTours();
        }
        
        public async Task<(Tour?, string)> AddTour(Tour tour)
        {
            return await api.AddTour(tour);
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
    }
}