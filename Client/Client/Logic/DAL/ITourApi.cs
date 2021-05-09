using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Model;

namespace Client.Logic.DAL
{
    public interface ITourApi
    {
        Task<(List<Tour>?, string)> GetTours();
        
        Task<(Tour?, string)> AddTour(Tour tour);
        
        Task<(List<Tour>?, string)> AddTours(List<Tour> tour);

        Task<(Tour?, string)> UpdateTour(Tour tour);

        Task<(bool, string)> DeleteTour(int id);

        Task<(BitmapImage?, BitmapImage?)> GetRouteImage(int id);
        
        Task<(bool, string)> GetExport(int id, string outputPath, bool isSummary = false);
    }
}