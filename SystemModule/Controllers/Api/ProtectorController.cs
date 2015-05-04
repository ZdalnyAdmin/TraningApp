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
            return db.Users.Where(x => x.Profile == ProfileEnum.Protector && !x.IsDeleted).OrderByDescending(p=>p.RegistrationDate);
        }


        public HttpResponseMessage Post(Person obj)
        {
            try
            {
                obj.RegistrationDate = DateTime.Now;
                obj.IsDeleted = false;


                if (ModelState.IsValid)
                {
                    db.Users.Add(obj);
                    db.SaveChanges();
                    LogService.InsertUserLogs(OperationLog.UserDelete, db, obj.Id, obj.ModifiedUserID);
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, obj);
                    return response;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(Person obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
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
