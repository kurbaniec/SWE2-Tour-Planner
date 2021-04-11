using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;
using Newtonsoft.Json;
using Server.BL;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace Server.Controllers
{
    [Controller]
    public class TourController
    {
        [Autowired]
        private TourPlannerServer tp = null!;
        
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
                Console.WriteLine(ex);
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