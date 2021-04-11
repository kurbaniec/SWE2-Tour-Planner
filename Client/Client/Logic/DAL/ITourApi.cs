using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Client.Logic.DAL
{
    public interface ITourApi
    {
        Task<(List<Tour>?, string)> GetTours();
        
        Task<(Tour?, string)> AddTour(Tour tour);

        Task<(Tour?, string)> UpdateTour(Tour tour);

        Task<(bool, string)> DeleteTour(int id);
    }
}