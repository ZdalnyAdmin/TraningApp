using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrganizationModule.Controllers.Api
{
    public class PeopleInGroupController : ApiController
    {
        private EFContext db = new EFContext(); 

        public HttpResponseMessage Post(List<ProfileGroup2Person> list)
        {
            if (ModelState.IsValid)
            {
                db.PeopleInGroups.AddRange(list);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, list);
                //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = group.ProfileGroupID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
