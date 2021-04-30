using System.Collections.Generic;
using Model;
using Server.DAL;
using WebService_Lib.Attributes;

namespace Server.BL
{
    [Component]
    public class TourPlannerServer
    {
        [Autowired]
        private IDataManagement db = null!;

        [Autowired]
        private IMapApi map = null!;

        public TourPlannerServer() {}
        
        public TourPlannerServer(IDataManagement db, IMapApi map)
        {
            this.db = db;
            this.map = map;
        }
        
        public List<Tour> GetTours()
        {
            return db.GetTours();
        }

        public (Tour?, string) AddTour(Tour tour)
        {
            if (string.IsNullOrEmpty(tour.To) || string.IsNullOrEmpty(tour.From))
                return (null, "To and From cannot be empty");
            var (result, errorMsg) = map.GetRouteInfo(tour.From, tour.To);
            if (result is { } mapApiResponse)
            {
                var kek = map.GetRouteInfo(tour.From, tour.To);
                return db.AddTour(tour);
            }

            return (null, errorMsg);
        }
        
        public (Tour?, string) UpdateTour(Tour tour)
        {
            return db.UpdateTour(tour);
        }

        public (bool, string) DeleteTour(int id)
        {
            return db.DeleteTour(id);
        }

        public string? GetRouteImage(int id)
        {
            return map.GetRouteImagePath(id);
        }
    }
}