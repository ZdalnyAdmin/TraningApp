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


        public HttpResponseMessage Post(Person obj)
        {
            try
            {
                obj.InvitationDate = DateTime.Now;
                //obj.Inviter = Person.GetLoggedPerson(User);
                obj.PhoneNumberConfirmed = false;
                obj.Profile = ProfileEnum.Protector;
                obj.Status = StatusEnum.Invited;
                obj.EmailConfirmed = false;
                obj.TwoFactorEnabled = false;
                obj.IsDeleted = false;

                obj.Organization = null;
                var organizationID = obj.OrganizationID.HasValue ? obj.OrganizationID : 0;
                obj.OrganizationID = null;

                if (ModelState.IsValid)
                {
                    db.Users.Add(obj);
                    db.SaveChanges();
                    if (organizationID != 0)
                    {
                        var organization = db.Organizations.FirstOrDefault(x => x.OrganizationID == organizationID);
                        organization.Protector = obj;
                        organization.ProtectorID = obj.Id;
                        db.Entry<Organization>(organization).State = EntityState.Modified;

                        obj.OrganizationID = organizationID;
                        obj.InviterID = Person.GetLoggedPerson(User).Id;
                        db.Entry<Person>(obj).State = EntityState.Modified;
                        db.SaveChanges();


                        LogService.ProtectorLogs(SystemLog.ProtectorInvitation, db, organization.Name, obj.InviterID);
                    }


                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, obj);
                    //add logs
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
