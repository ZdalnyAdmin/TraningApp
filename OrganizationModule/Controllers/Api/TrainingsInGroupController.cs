using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrganizationModule.Controllers.Api
{
    public class TrainingsInGroupController : ApiController
    {
        private EFContext db = new EFContext();

        public HttpResponseMessage Post(List<ProfileGroup2Trainings> list)
        {
            if (ModelState.IsValid)
            {
                db.TrainingInGroups.AddRange(list);
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
