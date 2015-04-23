using AppEngine.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrganizationModule.Controllers
{
    public class PersonController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            var list = new List<Person>();

            list.Add(new Person()
                {
                    PersonID = 1,
                    Login = "test",
                    Name = "Test Test",
                    Mail = "Test@mail.com",
                    Status = "aktywny",
                    RegistrationDate = DateTime.Now.Date,
                    LastActivationDate = DateTime.Now.Date,
                    GroupName = "test group",
                    ProfileName = "test role"
                });

            list.Add(new Person()
            {
                PersonID = 2,
                Login = "test2",
                Name = "Test Test2",
                Mail = "Test2@mail.com",
                Status = "aktywny",
                RegistrationDate = DateTime.Today.Date,
                LastActivationDate = DateTime.Now.Date,
                GroupName = "test group2",
                ProfileName = "test role2"
            });

            list.Add(new Person()
            {
                PersonID = 3,
                Login = "test3",
                Name = "Test Test3",
                Mail = "Test3@mail.com",
                Status = "aktywny",
                RegistrationDate = DateTime.Now.Date,
                LastActivationDate = DateTime.Now.Date,
                GroupName = "test group3",
                ProfileName = "test role3"
            });

            return list.AsEnumerable();
        }

        // GET api/<controller>/5
        public Person Get(int id)
        {
            return null;
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
        public HttpResponseMessage Put(int id, Person person)
        {

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            Person person = new Person();
            return Request.CreateResponse(HttpStatusCode.OK, person);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
