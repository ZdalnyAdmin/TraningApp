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
                switch (obj.ActionType)
                {
                    case PeopleActionType.GetProtectors:
                        obj.Current = new Person();

                        var items = db.Users.Where(x => x.Profile == ProfileEnum.Protector && x.Status == StatusEnum.Active && !x.IsDeleted).OrderByDescending(p => p.RegistrationDate).ToList();

                        foreach (var item in items)
                        {
                            item.Organization = db.Organizations.FirstOrDefault(x => x.ProtectorID == item.Id);
                            if (item.Organization != null)
                            {
                                item.OrganizationID = item.Organization.OrganizationID;
                            }
                        }

                        obj.People = items;

                        obj.Success = String.Empty;

                        break;
                    case PeopleActionType.DeleteProtector:

                        //check if user is assigned to organization
                        var deleted = obj.People.FirstOrDefault(x => x.Id == obj.Current.Id);
                        var deletedUsr = db.Users.FirstOrDefault(x => x.Id == obj.Current.Id);


                        if(deleted.OrganizationID.HasValue && deleted.OrganizationID.Value != 0)
                        {
                            obj.ErrorMessage = "Nie można usuną opiekuna przypisanego do organizacji. Proszę skontaktować się z administratorem.";
                            return Request.CreateResponse(HttpStatusCode.Created, obj);
                        }

                        deletedUsr.IsDeleted = true;

                        deletedUsr.DeleteUserID = Person.GetLoggedPerson(User).Id;
                        deletedUsr.DeletedDate = DateTime.Now;

                        db.Entry(deletedUsr).State = EntityState.Modified;

                        var organization = db.Organizations.FirstOrDefault(x => x.ProtectorID == obj.Current.Id);
                        if (organization != null)
                        {
                            organization.ProtectorID = null;
                            db.Entry(organization).State = EntityState.Modified;
                        }

                        db.SaveChanges();

                        obj.People.Remove(deleted);

                        obj.Current = null;

                        obj.Success = "Usuniecie opiekuna zakonczyla sie sukcesem!";

                        break;
                    case PeopleActionType.EditProtector:

                        var editable = obj.People.FirstOrDefault(x => x.Id == obj.Current.Id);
                        var user = db.Users.FirstOrDefault(x => x.Id == obj.Current.Id);
                        user.DisplayName = obj.Current.DisplayName;
                        user.Email = obj.Current.Email;

                        user.ModifiedUserID = Person.GetLoggedPerson(User).Id;
                        db.Entry(user).State = EntityState.Modified;


                        db.SaveChanges();

                        editable.UserName = obj.Current.UserName;
                        editable.Email = obj.Current.Email;

                        //LogService.InsertUserLogs(OperationLog.UserEdit, db, user.Id, user.ModifiedUserID, 0);

                        obj.Current = null;

                        obj.Success = "Edycja danych opiekuna zakonczyla sie sukcesem!";

                        break;
                    case PeopleActionType.AddProtector:
                        break;
                    case PeopleActionType.GetAdministrators:
                        obj.Current = new Person();

                        obj.People = db.Users.Where(x => x.Profile == ProfileEnum.Superuser && (x.Status == StatusEnum.Active || x.Status == StatusEnum.Blocked)).OrderByDescending(p => p.RegistrationDate).ToList();

                        if (obj.People != null && obj.People.Any())
                        {
                            foreach (var person in obj.People)
                            {
                                var lastActivation = (from t in db.Logs
                                                      where t.ModifiedUserID == person.Id
                                                      orderby t.ModifiedDate descending
                                                      select t).FirstOrDefault();
                                if(lastActivation != null)
                                {
                                    person.LastActivationDate = lastActivation.ModifiedDate;
                                }

                            }
                        }

                        obj.Success = String.Empty;
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
