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
    public class PersonController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Person> Get()
        {

            var people = db.Persons.AsEnumerable().ToList();

            foreach (var item in people)
            {
                item.Status = (from t in db.Status
                               where t.StatusID == item.StatusID
                               select t).FirstOrDefault();

                item.Profile = (from t in db.Profiles
                                where t.ProfileID == item.ProfileID
                                select t).FirstOrDefault();

                //var groups = (from t in db.Groups
                //              where t.ProfileGroupID == item.ProfileGroupID
                //              select t).ToList();

                //if (groups != null && groups.Any())
                //{
                //    item.GroupName = string.Join(",", groups);
                //}

                item.AssignedTrainings = (from t in db.TrainingResults
                                          where t.PersonID == item.PersonID
                                          select t).ToList();
            }


            return people;
        }

        // GET api/<controller>/5
        public Person Get(int id)
        {
            var person = db.Persons.Find(id);
            if (person == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return person;
        }

        // POST api/<controller>
        public HttpResponseMessage Post(Person person)
        {
            if (ModelState.IsValid)
            {

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, person);
                //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = Contact.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(Person person)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            //if (id != person.PersonID)
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest);
            //}
            if(person.Status != null && person.Status.StatusID != person.StatusID)
            {
                person.StatusID = person.Status.StatusID;
            }



            db.Entry(person).State = EntityState.Modified;

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
        public HttpResponseMessage Delete(int id)
        {
            var person = db.Persons.Find(id);
            if (person == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Persons.Remove(person);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, person);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
