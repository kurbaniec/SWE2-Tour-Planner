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
        [Autowired] private readonly IExportHandler handler = null!;
        private readonly ILogger logger = WebServiceLogging.CreateLogger<TourPlannerServer>();

        // ReSharper disable once UnusedMember.Global
        public TourPlannerServer()
        {
        }

        // ReSharper disable once UnusedMember.Global
        public TourPlannerServer(IDataManagement db, IMapApi map, IExportHandler handler)
        {
            this.db = db;
            this.map = map;
            this.handler = handler;
        }

        /// <summary>
        /// Query all Tours from the configured Data Access Layer (DAL).
        /// </summary>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the queried Tours as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        public (List<Tour>?, string) GetTours()
        {
            logger.Log(LogLevel.Information, "Returning all Tours");
            return db.GetTours();
        }

        /// <summary>
        /// Adds a new Tour in the configured DAL.
        /// </summary>
        /// <param name="tour">
        /// Tour which should be added.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the added Tour as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
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

        /// <summary>
        /// Adds multiple new Tours in the configured DAL.
        /// </summary>
        /// <param name="tours">
        /// Tours which should be added.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the added Tours as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        public (List<Tour>?, string) AddTours(List<Tour> tours)
        {
            var newTours = new List<Tour>();
            foreach (var tour in tours)
            {
                var (newTour, errorMsg) = AddTour(tour);
                if (newTour is { })
                    newTours.Add(newTour);
                else
                    return (null, errorMsg);
            }

            return (newTours, string.Empty);
        }

        /// <summary>
        /// Updates an existing Tour in the configured DAL.
        /// </summary>
        /// <param name="tour">
        /// Tour which should be updated.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the updated Tour as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
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

            var (oldTour, _) = db.GetTour(tour.Id);
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

        /// <summary>
        /// Deletes an existing Tour in the configured DAL.
        /// </summary>
        /// <param name="id">
        /// The id of the Tour which should be deleted.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the true as item1 and a empty string as item2 is returned.
        /// On failure item1 is false and item2 will contain the error message.
        /// </returns>
        public (bool, string) DeleteTour(int id)
        {
            logger.Log(LogLevel.Information, $"Trying to delete Tour with id {id}");
            return db.DeleteTour(id);
        }
        
        /// <summary>
        /// Requests the Route Image of an existing Tour in the configured DAL.
        /// </summary>
        /// <param name="id">
        /// Id of the Tour of whom the Route Image is requested.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the Route Image as item1 and null as item2 is returned.
        /// On failure item1 is null and item2 will contain a placeholder image.
        /// </returns>
        public (string?, string) GetRouteImage(int id)
        {
            logger.Log(LogLevel.Information,
                $"Trying to request route information image for Tour with id {id}");
            return map.GetRouteImagePath(id);
        }

        /// <summary>
        /// Exports a printable document from a given Tour.
        /// </summary>
        /// <param name="id">
        /// Id of the Tour that should be printed.
        /// </param>
        /// <param name="isSummary">
        /// Determines if a summary or full report is generated.
        /// </param>
        /// <returns>
        /// Returns a tuple.
        /// On success a tuple with the path to the document as item1 and a empty string as item2 is returned.
        /// On failure item1 is null and item2 will contain the error message.
        /// </returns>
        public (string?, string) GetPdfExport(int id, bool isSummary = false)
        {
            var (tour, dbError) = db.GetTour(id);
            if (tour is null) return (null, dbError);
            var (imagePath, _) = GetRouteImage(id);
            return handler.Export(tour, imagePath, isSummary);
        }
    }
}