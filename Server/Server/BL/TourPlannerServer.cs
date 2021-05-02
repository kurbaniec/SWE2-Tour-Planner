using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Model;
using Server.DAL;
using WebService_Lib.Attributes;
using WebService_Lib.Logging;

namespace Server.BL
{
    [Component]
    public class TourPlannerServer
    {
        [Autowired] private readonly IDataManagement db = null!;
        [Autowired] private readonly IMapApi map = null!;
        private readonly ILogger logger = WebServiceLogging.CreateLogger<TourPlannerServer>();

        // ReSharper disable once UnusedMember.Global
        public TourPlannerServer()
        {
        }

        // ReSharper disable once UnusedMember.Global
        public TourPlannerServer(IDataManagement db, IMapApi map)
        {
            this.db = db;
            this.map = map;
        }

        public List<Tour> GetTours()
        {
            logger.Log(LogLevel.Information, "Returning all Tours");
            return db.GetTours();
        }

        public (Tour?, string) AddTour(Tour tour)
        {
            logger.Log(LogLevel.Information, "Trying to add new Tour");
            if (string.IsNullOrEmpty(tour.To) || string.IsNullOrEmpty(tour.From))
            {
                logger.Log(LogLevel.Error, "Received unsupported Tour data");
                logger.Log(LogLevel.Error, "To and From cannot be empty");
                return (null, "To and From cannot be empty");
            }
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
            logger.Log(LogLevel.Information, $"Trying to update Tour with id {tour.Id}");
            if (string.IsNullOrEmpty(tour.To) || string.IsNullOrEmpty(tour.From))
            {
                logger.Log(LogLevel.Error, "Received unsupported Tour data");
                logger.Log(LogLevel.Error, "To and From cannot be empty");
                return (null, "To and From cannot be empty");
            }

            if (tour.Id == 0)
            {
                logger.Log(LogLevel.Error, "Received unsupported Tour data");
                logger.Log(LogLevel.Error, "Id cannot be 0");
                return (null, "Id cannot be 0");
            }

            var oldTour = db.GetTour(tour.Id);
            if (oldTour is null)
            {
                logger.Log(LogLevel.Error, "Invalid tour id given");
                return (null, "Invalid tour id given");
            }

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
            logger.Log(LogLevel.Information, $"Trying to delete Tour with id {id}");
            return db.DeleteTour(id);
        }

        public string? GetRouteImage(int id)
        {
            logger.Log(LogLevel.Information, 
                $"Trying to request route information image for Tour with id {id}");
            return map.GetRouteImagePath(id);
        }
    }
}