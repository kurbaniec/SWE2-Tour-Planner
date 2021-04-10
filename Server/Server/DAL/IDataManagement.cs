using System.Collections.Generic;
using Model;

namespace Server.DAL
{
    public interface IDataManagement
    {
        List<Tour> GetTours();

        int? AddTour(Tour tour);

        bool UpdateTour(Tour tour);

        bool DeleteTour(int id);

        int? AddTourLog(int tourId, TourLog log);

        bool UpdateTourLog(int tourId, TourLog log);

        bool DeleteTourLog(int tourId, TourLog log);
    }
}