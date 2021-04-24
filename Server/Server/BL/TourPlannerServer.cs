﻿using System.Collections.Generic;
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
            return db.AddTour(tour);
        }
        
        public (Tour?, string) UpdateTour(Tour tour)
        {
            return db.UpdateTour(tour);
        }

        public (bool, string) DeleteTour(int id)
        {
            return db.DeleteTour(id);
        }

        public bool SaveRouteImage(string from, string to, string id)
        {
            return map.SaveRouteImage(from, to, id);
        }

        public string? GetRouteImage(int id)
        {
            return map.GetRouteImagePath(id);
        }
    }
}