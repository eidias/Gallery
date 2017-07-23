using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Gallery.Web.ApiControllers
{
    public class DefaultApiController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Status()
        {
            return Ok();
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            List<int> numbers = new List<int> { 1, 2, 3 };
            return Request.CreateResponse(HttpStatusCode.OK, numbers);
        }
    }
}
