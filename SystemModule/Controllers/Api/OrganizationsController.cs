using AppEngine.Helpers;
using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
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
    public class OrganizationsController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Organization> Get()
        {
            return db.Organizations.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreateDate);
        }

        public HttpResponseMessage Post(Organization obj)
        {
            try
            {
                //init object
                if(obj.OrganizationID == -1)
                {
                    var setting = db.AppSettings.FirstOrDefault(x => x.IsDefault);
                    if(setting == null)
                    {
                        setting = new AppSetting();
                        setting.AllowUserToChangeName = true;
                        setting.AllowUserToChangeMail = true;
                        setting.SpaceDisk =  50;
                        setting.MaxAssignedUser =  10;
                        setting.IsGlobalAvailable = true;
                        setting.IsTrainingAvailableForAll = true;
                        setting.MaxActiveTrainings = 5;
                        setting.DefaultEmail = string.Empty;
                        setting.DefaultName =  string.Empty;
                        setting.IsDefault = true;
                        db.AppSettings.Add(setting);
                        db.SaveChanges();
                    }

                    obj.SpaceDisk = setting.SpaceDisk;
                    obj.MaxAssignedUser = setting.MaxAssignedUser;
                    obj.IsGlobalAvailable = setting.IsGlobalAvailable;
                    obj.IsTrainingAvailableForAll = setting.IsTrainingAvailableForAll;
                    obj.CanUserChangeMail = setting.AllowUserToChangeMail;
                    obj.CanUserChangeName = setting.AllowUserToChangeName;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, obj);
                    return response;
                }

                var usr = Person.GetLoggedPerson(User);
                obj.CreateUserID = Helpers.GetUserID(usr);
                obj.CreateDate = DateTime.Now;
                obj.IsDeleted = false;

                if (ModelState.IsValid)
                {
                    db.Organizations.Add(obj);
                    db.SaveChanges();
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
        public HttpResponseMessage Put(Organization obj)
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
                //LogService.InsertTrainingLogs(OperationLog.TrainingEdit, db, obj.TrainingID, obj.CreateUserID);
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
