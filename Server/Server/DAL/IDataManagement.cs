using System.Collections.Generic;
using Model;

namespace Server.DAL
{
    public interface IDataManagement
    {
        (List<Tour>?, string) GetTours();

        (Tour?, string) GetTour(int id);

        (Tour?, string) AddTour(Tour tour);

        (Tour?, string) UpdateTour(Tour tour);

        (bool, string) DeleteTour(int id);
    }
}