using AppEngine.Models;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SystemModule.Controllers.Api
{
    public class ProtectorController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            var items  = db.Users.Where(x => x.Profile == ProfileEnum.Protector && !x.IsDeleted).OrderByDescending(p => p.RegistrationDate).ToList();

            foreach (var item in items)
            {
                item.Organization = db.Organizations.FirstOrDefault(x => x.ProtectorID == item.Id);
                if (item.Organization != null)
                {
                    item.OrganizationID = item.Organization.OrganizationID;
                }
            }

            return items;
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(Person obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var usr = Person.GetLoggedPerson(User);
            if (obj.IsDeleted)
            {
                obj.DeleteUserID = usr.Id;
                obj.DeletedDate = DateTime.Now;
            }
            else
            {
                obj.ModifiedUserID = usr.Id;
            }

            db.Entry(obj).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                LogService.InsertUserLogs(OperationLog.UserEdit, db, obj.Id, obj.ModifiedUserID);
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
