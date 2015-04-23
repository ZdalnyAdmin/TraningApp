using AppEngine.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrganizationModule.Controllers
{
    public class TraningController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Training> Get()
        {
            var list = new List<Training>();

            list.Add(new Training()
            {
                TraninigID = 1,
                Name = "Test training 1",
                CreateDate = DateTime.Today.Date,
                CreateUserID = 1,
                TraningTypeID = 1
            });

            list.Add(new Training()
            {
                TraninigID = 2,
                Name = "Test training 2",
                CreateDate = DateTime.Today.Date,
                CreateUserID = 1,
                TraningTypeID = 1
            });

            list.Add(new Training()
            {
                TraninigID = 3,
                Name = "Test training 3",
                CreateDate = DateTime.Today.Date,
                CreateUserID = 1,
                TraningTypeID = 2
            });

            return list.AsEnumerable();
        }

        // GET api/<controller>/5
        public Training Get(int id)
        {
            return null;
        }

        // POST api/<controller>
        public HttpResponseMessage Post(Training obj)
        {
            if (ModelState.IsValid)
            {

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, obj);
                //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = Contact.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, Training trainig)
        {

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            Training obj = new Training();
            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
