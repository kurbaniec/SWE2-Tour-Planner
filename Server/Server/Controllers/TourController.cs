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
        private TourPlanner tp = null!;
        
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
    }
}