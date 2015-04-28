using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrganizationModule.Controllers.Api
{
    public class TrainingController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Training> Get()
        {
            //get from correct profile
            var list = db.Trainings.ToList();
            if(list == null)
            {
                return null;
            }
            foreach(var item in list)
            {
                var user = db.Persons.FirstOrDefault(x => x.PersonID == item.CreateUserID);
                if(user == null)
                {
                    continue;
                }
                item.SetCreateUserName(user.Name);
                var runCounter = db.TrainingResults.Count(x => x.TrainingID == item.TrainingID);
                item.SetRunTrainingStats(runCounter);

                var groups = (from pg in db.TrainingInGroups
                              join g in db.Groups on pg.ProfileGroupID equals g.ProfileGroupID
                              where pg.TrainingID == item.TrainingID
                              select g.Name).ToList();
                item.SetAssignedGroups(groups);
            }

            return list;
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(Training obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            db.Entry(obj).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
