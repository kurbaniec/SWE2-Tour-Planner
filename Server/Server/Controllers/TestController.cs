using System.Collections.Generic;
using Server.Components;
using WebService_Lib.Attributes;
using WebService_Lib.Attributes.Rest;
using WebService_Lib.Server;

namespace Server.Controllers
{
    [Controller]
    public class TestController
    {
        [Autowired]
        private ITest test = null!;
        
        [Get("/test")]
        public Response Test()
        {
            test.Hi();
            return Response.Json(new Dictionary<string, object>()
            {
                {"message","Hi!"}
            });
        }
    }
}