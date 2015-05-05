using AppEngine.Helpers;
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

namespace OrganizationModule.Controllers
{
    public class GroupController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<ProfileGroup> Get()
        {
            var groups = db.Groups.Where(x => !x.IsDeleted).ToList();

            foreach(var item in groups)
            {
                var people = (from pg in db.PeopleInGroups
                              join p in db.Users on pg.PersonID equals p.Id
                              where pg.ProfileGroupID == item.ProfileGroupID
                              select p).ToList();
                item.SetAssignedPeople(people);
            }
            return groups;
        }

        // POST api/<controller>
        public HttpResponseMessage Post(ProfileGroup group)
        {
            group.CreateDate = DateTime.Now;
            group.DeletedUserID = -1;
            group.IsDeleted = false;
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, group);
                //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = group.ProfileGroupID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(ProfileGroup obj)
        {

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            if(obj.IsDeleted)
            {
                var usr = Person.GetLoggedPerson(User);
                obj.DeletedUserID = Helpers.GetUserID(usr);
                obj.DeletedDate = DateTime.Now;
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

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(ProfileGroup group)
        {
            if (group == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, group);
            }


            var obj = db.Groups.Find(group.ProfileGroupID);
            if (obj == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Groups.Remove(obj);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, obj);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
