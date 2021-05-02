using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Model;
using Newtonsoft.Json;
using Server.BL;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Logging;
using WebService_Lib.Server;

namespace Server.Controllers
{
    [Controller]
    public class TourController
    {
        [Autowired] private readonly TourPlannerServer tp = null!;
        private readonly ILogger logger = WebServiceLogging.CreateLogger<TourController>();

        [Get("/api/tours")]
        public Response GetTours()
        {
            string jsonString;
            try
            {
                var tours = tp.GetTours();
                jsonString = JsonConvert.SerializeObject(tours);

                // TOOD use later to convert dictionary
                //var json = JsonConvert.DeserializeObject<Dictionary<string, Object>>(jsonString);
                //var jsonString2 = JsonConvert.SerializeObject(json);
                //var tourback = JsonConvert.DeserializeObject<Tour>(jsonString2);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, $"Encountered exception in {MethodBase.GetCurrentMethod()}:");
                logger.Log(LogLevel.Error, ex.StackTrace);
                return Response.Status(Status.BadRequest);
            }

            return Response.Json(jsonString);
        }

        [Get("/api/route")]
        public Response GetRouteImage(PathVariable<int> id)
        {
            if (id.Ok)
            {
                var imagePath = tp.GetRouteImage(id.Value!);
                if (imagePath is { } path)
                {
                    var response = Response.File(path);
                    if (response is { }) return response;
                }
            }
            return Response.PlainText("Route Image not found", Status.NotFound);
        }

        [Post("/api/tour")]
        public Response AddTour([JsonString] string json)
        {
            try
            {
                // Parse received tour
                Tour tour;
                var parsedTour = JsonConvert.DeserializeObject<Tour>(json);
                if (parsedTour is { } actualTour)
                    tour = actualTour;
                else
                {
                    return Response.PlainText("Invalid payload", Status.BadRequest);
                }

                // Add tour
                var (newTour, errorMsg) = tp.AddTour(tour);
                // Check result
                if (newTour is null) return Response.PlainText(errorMsg, Status.BadRequest);
                var jsonString = JsonConvert.SerializeObject(newTour);
                return Response.Json(jsonString);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, $"Encountered exception in {MethodBase.GetCurrentMethod()}:");
                logger.Log(LogLevel.Error, ex.StackTrace);
                return Response.PlainText("Invalid payload", Status.BadRequest);
            }
        }

        [Put("/api/tour")]
        public Response UpdateTour([JsonString] string json)
        {
            try
            {
                // Parse received tour
                Tour tour;
                var parsedTour = JsonConvert.DeserializeObject<Tour>(json);
                if (parsedTour is { } actualTour)
                    tour = actualTour;
                else
                {
                    return Response.PlainText("Invalid payload", Status.BadRequest);
                }

                // Update tour
                var (updatedTour, errorMsg) = tp.UpdateTour(tour);
                // Check result
                if (updatedTour is null) return Response.PlainText(errorMsg, Status.BadRequest);
                var jsonString = JsonConvert.SerializeObject(updatedTour);
                return Response.Json(jsonString);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, $"Encountered exception in {MethodBase.GetCurrentMethod()}:");
                logger.Log(LogLevel.Error, ex.StackTrace);
                return Response.PlainText("Invalid payload", Status.BadRequest);
            }
        }
        
        [Delete("/api/tour")]
        public Response DeleteTour(PathVariable<int> id)
        {
            if (id.Ok)
            {
                var (isDeleted, errorMsg) = tp.DeleteTour(id.Value!);
                return isDeleted ? 
                    Response.Status(Status.NoContent) : 
                    Response.PlainText(errorMsg, Status.BadRequest);
            }
            return Response.PlainText("Not valid Route id given", Status.BadRequest);
        }
    }
}