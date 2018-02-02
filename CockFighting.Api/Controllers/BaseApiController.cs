using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CockFighting.OnePage.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseApiController : ApiController
    {
         protected ApiResult<T> GetResult<T>(int status, T data, string responseKey = "")
        {
            ApiResult<T> result = new ApiResult<T>()
            {
                status = status,
                data = data,
                responseKey = responseKey
            };

            return result;
        }

        public class ApiResult<T>
        {
            public int status { get; set; }
            public string responseKey { get; set; }
            public T data { get; set; }
            public string error { get; set; }
            public List<string> errors { get; set; }
        }
    }
}
