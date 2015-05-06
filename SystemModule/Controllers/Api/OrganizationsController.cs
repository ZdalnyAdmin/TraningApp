using AppEngine.Helpers;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
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
            return db.Organizations.OrderByDescending(x => x.CreateDate);
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
                obj.CreateUserID = usr.Id;
                obj.CreateDate = DateTime.Now;
                obj.IsDeleted = false;
                obj.Status = OrganizationEnum.Active;
                if (ModelState.IsValid)
                {
                    db.Organizations.Add(obj);
                    db.SaveChanges();
                    LogService.OrganizationLogs(SystemLog.OrganizationCreate, db, obj.Name, obj.CreateUserID);
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
                obj.DeletedUserID = usr.Id;
                obj.DeletedDate = DateTime.Now;
                obj.Status = OrganizationEnum.Deleted;
            }

            db.Entry(obj).State = EntityState.Modified;

            try
            {

                db.SaveChanges();

                if(obj.IsDeleted)
                {
                    LogService.OrganizationLogs(SystemLog.OrganizationRequestToRemove, db, obj.Name, obj.CreateUserID);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.Created, obj);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
