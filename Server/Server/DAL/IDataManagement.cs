using System.Collections.Generic;
using Model;

namespace Server.DAL
{
    public interface IDataManagement
    {
        List<Tour> GetTours();

        bool UpdateTour(Tour tour);

        bool DeleteTour(int id);
    }
}