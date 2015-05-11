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

            foreach (var item in groups)
            {
                var people = (from pg in db.PeopleInGroups
                              join p in db.Users on pg.PersonID equals p.Id
                              where pg.ProfileGroupID == item.ProfileGroupID
                              select p).ToList();
                item.AssignedPeople = people;
            }
            return groups;
        }

        // POST api/<controller>
        [HttpPost]
        public HttpResponseMessage Post(ProfileGroup obj)
        {
            obj.CreateDate = DateTime.Now;
            var usr = Person.GetLoggedPerson(User);
            obj.CreateUserID = usr.Id;
            obj.IsDeleted = false;
            if (ModelState.IsValid)
            {
                db.Groups.Add(obj);
                db.SaveChanges();

                if (obj.AssignedPeople != null)
                {
                    foreach (var item in obj.AssignedPeople)
                    {
                        var pg = new ProfileGroup2Person();
                        pg.IsDeleted = false;
                        pg.PersonID = item.Id;
                        pg.ProfileGroupID = obj.ProfileGroupID;
                        db.PeopleInGroups.Add(pg);
                    }
                    db.SaveChanges();
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, obj);
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


            if (obj.IsDeleted)
            {
                var usr = Person.GetLoggedPerson(User);
                obj.DeletedUserID = usr.Id;
                obj.DeletedDate = DateTime.Now;

                var personsInGroups = (from t in db.PeopleInGroups
                                       where t.ProfileGroupID == obj.ProfileGroupID
                                       select t).ToList();
                if (personsInGroups != null && personsInGroups.Any())
                {
                    db.PeopleInGroups.RemoveRange(personsInGroups);
                }
            }
            else
            {
                var personsInGroups = (from t in db.PeopleInGroups
                                       where t.ProfileGroupID == obj.ProfileGroupID
                                       select t).ToList();
                if (personsInGroups != null && personsInGroups.Any())
                {
                    db.PeopleInGroups.RemoveRange(personsInGroups);
                }

                if (obj.AssignedPeople != null && obj.AssignedPeople.Any())
                {
                    foreach (var item in obj.AssignedPeople)
                    {
                        var pg = new ProfileGroup2Person();
                        pg.IsDeleted = false;
                        pg.PersonID = item.Id;
                        pg.ProfileGroupID = obj.ProfileGroupID;
                        db.PeopleInGroups.Add(pg);
                    }
                }
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
