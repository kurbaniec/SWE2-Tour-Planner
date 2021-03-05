using System.Collections.Generic;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace Server.Controllers
{
    [Controller]
    public class TestController
    {
        [Get("/test")]
        public Response Test()
        {
            return Response.Json(new Dictionary<string, object>()
            {
                {"message","Hi!"}
            });
        }
    }
}