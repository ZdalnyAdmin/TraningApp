using AppEngine.Models.DataBusiness;
using AppEngine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SystemModule.Controllers.Api
{
    public class LocalFileController : ApiController
    {
        
        // GET api/<controller>
        // POST api/<controller>
        public HttpResponseMessage Post(UserImage obj)
        {
            //logic will be move to service
            if (obj == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            var files = ImageService.LoadImage(obj);


            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, files);
            //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = group.ProfileGroupID }));
            return response;

        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(UserImage obj)
        {


            return Request.CreateResponse(HttpStatusCode.OK);
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
