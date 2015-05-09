using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
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
    public class SettingsController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<AppSetting> Get()
        {
            //get from correct profil
            return db.AppSettings.AsEnumerable();
        }


        // PUT api/<controller>/5
        public HttpResponseMessage Put(AppSetting obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            //only system can change for true;
            obj.IsDefault = false;

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

        // POST api/<controller>
        public HttpResponseMessage Post(AppSetting obj)
        {
            //check if object exist 
            if (obj == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if(obj.ProtectorID == "-1")
            {
                obj.Protector = Person.GetLoggedPerson(User);
            }

            //one settings per single protector
            if (obj.Protector != null)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, obj.Protector);
                return response;
            }

            var global = db.AppSettings.FirstOrDefault(x => x.IsDefault);

            if (ModelState.IsValid)
            {
                //default settings
                obj.AllowUserToChangeName = true;
                obj.AllowUserToChangeMail = true;
                obj.SpaceDisk = global != null ? global.SpaceDisk : 50;
                obj.MaxAssignedUser = global != null ? global.MaxAssignedUser : 10;
                obj.IsGlobalAvailable = true;
                obj.IsTrainingAvailableForAll = true;
                obj.MaxActiveTrainings = 5;
                obj.DefaultEmail = global != null ? global.DefaultEmail : string.Empty;
                obj.DefaultName = global != null ? global.DefaultName : string.Empty;
                db.AppSettings.Add(obj);
                db.SaveChanges();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, obj);
                //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = Contact.Id }));
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
