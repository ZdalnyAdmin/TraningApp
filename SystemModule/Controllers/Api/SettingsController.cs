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

namespace SystemModule.Controllers.Api
{
    public class SettingsController : ApiController
    {
        private EFContext db = new EFContext();

        // PUT api/<controller>/5
        public HttpResponseMessage Put(AppSetting obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            obj.IsDefault = true;
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

            var global = db.AppSettings.FirstOrDefault(x => x.IsDefault);
            //one settings per single protector
            if (global != null)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, global);
                return response;
            }

            if (ModelState.IsValid)
            {
                //default settings
                obj.AllowUserToChangeName = true;
                obj.AllowUserToChangeMail = true;
                obj.SpaceDisk = 50;
                obj.MaxAssignedUser = 10;
                obj.IsGlobalAvailable = true;
                obj.IsTrainingAvailableForAll = true;
                obj.MaxActiveTrainings = 5;
                obj.DefaultEmail = "support@kenpro.pl";
                obj.DefaultName = "Kenpro";
                obj.IsDefault = true;
                db.AppSettings.Add(obj);
                db.SaveChanges();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, obj);
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }
    }
}
