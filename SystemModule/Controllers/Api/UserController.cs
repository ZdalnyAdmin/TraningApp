using AppEngine.Models;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Services;
using AppEngine.ViewModels;
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
    public class UserController : ApiController
    {
        private EFContext db = new EFContext();

        [HttpPost]
        public HttpResponseMessage Post(PersonViewModel obj)
        {

            try
            {
                obj.Current = new Person();

                switch (obj.ActionType)
                {
                    case PeopleActionType.GetProtectors:

                        var items = db.Users.Where(x => x.Profile == ProfileEnum.Protector && !x.IsDeleted).OrderByDescending(p => p.RegistrationDate).ToList();

                        foreach (var item in items)
                        {
                            item.Organization = db.Organizations.FirstOrDefault(x => x.ProtectorID == item.Id);
                            if (item.Organization != null)
                            {
                                item.OrganizationID = item.Organization.OrganizationID;
                            }
                        }

                        obj.People = items;

                        break;
                    case PeopleActionType.DeleteProtector:

                        var current = obj.People.FirstOrDefault(x => x.Id == obj.Current.Id);

                        obj.Current.IsDeleted = true;

                        obj.Current.DeleteUserID = Person.GetLoggedPerson(User).Id;
                        obj.Current.DeletedDate = DateTime.Now;

                        db.Entry(obj.Current).State = EntityState.Modified;

                        var organization = db.Organizations.FirstOrDefault(x => x.ProtectorID == obj.Current.Id);
                        if (organization != null)
                        {
                            organization.ProtectorID = null;
                            db.Entry(organization).State = EntityState.Modified;
                        }

                        db.SaveChanges();

                        obj.People.Remove(current);

                        obj.Current = null;

                        break;
                    case PeopleActionType.EditProtector:

                        var editable = obj.People.FirstOrDefault(x => x.Id == obj.Current.Id);

                        obj.Current.ModifiedUserID = Person.GetLoggedPerson(User).Id;


                        db.Entry(obj.Current).State = EntityState.Modified;


                        db.SaveChanges();

                        editable.UserName = obj.Current.UserName;
                        editable.Email = obj.Current.Email;

                        LogService.InsertUserLogs(OperationLog.UserEdit, db, obj.Current.Id, obj.Current.ModifiedUserID);

                        obj.Current = null;

                        break;
                    case PeopleActionType.AddProtector:
                        break;
                    case PeopleActionType.GetAdministrators:

                        obj.People = db.Users.Where(x => x.Profile == ProfileEnum.Administrator && !x.IsDeleted).OrderByDescending(p => p.RegistrationDate).ToList();

                        break;
                    default:
                        break;
                }



                return Request.CreateResponse(HttpStatusCode.Created, obj);
            }
            catch (Exception ex)
            {
                obj.ErrorMessage = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, obj.ErrorMessage);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
