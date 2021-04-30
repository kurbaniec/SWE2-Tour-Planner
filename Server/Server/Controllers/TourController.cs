using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        [Autowired]
        private TourPlannerServer tp = null!;

        private ILogger logger = WebServiceLogging.CreateLogger<TourController>();
        
        [Get("/api/tours")]
        public Response GetTours()
        {
            var jsonString = "";
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

            return Response.Status(Status.NotFound);
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
                var result = tp.AddTour(tour);
                // Check result
                if (result.Item1 is { } newTour)
                {
                    var jsonString = JsonConvert.SerializeObject(newTour);
                    return Response.Json(jsonString);
                }
                return Response.PlainText(result.Item2, Status.BadRequest);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Error, $"Encountered exception in {MethodBase.GetCurrentMethod()}:");
                logger.Log(LogLevel.Error, ex.StackTrace);
                return Response.PlainText("Invalid payload", Status.BadRequest);
            }
        }
        

        [Post("/api/test")]
        public Response Test([JsonString] string json)
        {
            Console.WriteLine(json);
            return Response.Status(Status.Ok);
        }
        
        [Post("/api/test2")]
        public Response Test2(Dictionary<string, object>? json)
        {
            Console.WriteLine(json);
            return Response.Status(Status.Ok);
        }
        
        [Post("/api/test3")]
        public Response Test2([JsonString] string jsonString, Dictionary<string, object>? json)
        {
            Console.WriteLine(jsonString);
            Console.WriteLine(json);
            return Response.Status(Status.Ok);
        }
    }
}