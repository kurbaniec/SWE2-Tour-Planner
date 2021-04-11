using System.Collections.Generic;
using Model;
using Server.DAL;
using WebService_Lib.Attributes;

namespace Server.BL
{
    [Component]
    public class TourPlanner
    {
        [Autowired]
        private IDataManagement db = null!;

        [Autowired]
        private IMapApi map = null!;

        public TourPlanner() {}
        
        public TourPlanner(IDataManagement db, IMapApi map)
        {
            this.db = db;
            this.map = map;
        }
        
        public List<Tour> GetTours()
        {
            return db.GetTours();
        }

        public bool AddTour(Tour tour)
        {
            return db.AddTour(tour);
        }
        
        public bool UpdateTour(Tour tour)
        {
            return db.UpdateTour(tour);
        }

        public bool DeleteTour(int id)
        {
            return db.DeleteTour(id);
        }

        /*
        int? AddTourLog(int tourId, TourLog log)
        {
            return db.AddTourLog(tourId, log);
        }

        bool UpdateTourLog(int tourId, TourLog log)
        {
            return db.UpdateTourLog(tourId, log);
        }

        bool DeleteTourLog(int tourId, TourLog log)
        {
            return db.DeleteTourLog(tourId, log);
        }*/

        public bool SaveRouteImage(string from, string to, string id)
        {
            return map.SaveRouteImage(from, to, id);
        }

        public string? GetRouteImage(string id)
        {
            return map.GetRouteImagePath(id);
        }
    }
}