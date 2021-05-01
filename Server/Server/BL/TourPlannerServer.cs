using System.Collections.Generic;
using Model;
using Server.DAL;
using WebService_Lib.Attributes;

namespace Server.BL
{
    [Component]
    public class TourPlannerServer
    {
        [Autowired] private IDataManagement db = null!;

        [Autowired] private IMapApi map = null!;

        public TourPlannerServer()
        {
        }

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

            var (mapApiResponse, mapError) = map.GetRouteInfo(tour.From, tour.To);
            if (mapApiResponse is { })
            {
                tour.Distance = mapApiResponse.Distance;
                var (newTour, dbError) = db.AddTour(tour);
                if (newTour is { })
                {
                    var (routeApiResponse, routeError)
                        = map.SaveRouteImage(newTour.Id, mapApiResponse.BoundingBox, mapApiResponse.SessionId);
                    return routeApiResponse ? (newTour, string.Empty) : (null, routeError);
                }

                return (null, dbError);
            }

            return (null, mapError);
        }

        public (Tour?, string) UpdateTour(Tour tour)
        {
            if (string.IsNullOrEmpty(tour.To) || string.IsNullOrEmpty(tour.From))
                return (null, "To and From cannot be empty");
            if (tour.Id == 0)
                return (null, "Id cannot be 0");
            var oldTour = db.GetTour(tour.Id);
            if (oldTour is null)
                return (null, "Invalid tour id given");
            if (tour.From != oldTour.From || tour.To != oldTour.To)
            {
                // Update Distance & Route information when changed
                var (mapApiResponse, mapError) = map.GetRouteInfo(tour.From, tour.To);
                if (mapApiResponse is { })
                {
                    tour.Distance = mapApiResponse.Distance;
                    var (updatedTour, dbError) = db.UpdateTour(tour);
                    if (updatedTour is { })
                    {
                        var (routeApiResponse, routeError)
                            = map.SaveRouteImage(updatedTour.Id, mapApiResponse.BoundingBox, mapApiResponse.SessionId);
                        return routeApiResponse ? (newTour: updatedTour, string.Empty) : (null, routeError);
                    }

                    return (null, dbError);
                }

                return (null, mapError);
            }
            
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