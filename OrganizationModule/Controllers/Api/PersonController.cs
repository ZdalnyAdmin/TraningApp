using AppEngine.Helpers;
using AppEngine.Models;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DTO;
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

namespace OrganizationModule.Controllers
{
    public class PersonController : ApiController
    {
        private EFContext db = new EFContext();

        // POST api/<controller>
        [HttpPost]
        public HttpResponseMessage Post(UserManagmentViewModel obj)
        {
            try
            {
                if (obj.LoggedUser == null)
                {
                    obj.LoggedUser = Person.GetLoggedPerson(User);
                }

                if (obj.CurrentOrganization == null)
                {
                    obj.CurrentOrganization = db.Organizations.FirstOrDefault(x => x.OrganizationID == obj.LoggedUser.OrganizationID);
                }

                if (obj.CurrentOrganization == null)
                {
                    obj.ErrorMessage = "Brak organizacji dla której można pobrać użytkowników";
                    return Request.CreateResponse(HttpStatusCode.Created, obj);
                }

                switch (obj.ActionType)
                {
                    case BaseActionType.Get:
                        //todo logs for add and send invitation
                        var people = (from p in db.Users
                                      where
                                      p.OrganizationID == obj.CurrentOrganization.OrganizationID &&
                                      (p.Status == StatusEnum.Active || p.Status == StatusEnum.Blocked || p.Status == StatusEnum.Deleted)
                                      && (p.Profile == ProfileEnum.User || p.Profile == ProfileEnum.Creator || p.Profile == ProfileEnum.Administrator || p.Profile == ProfileEnum.Manager)
                                      orderby p.RegistrationDate
                                      select p).ToList();


                        var groups = (from gio in db.Groups
                                      join g in db.GroupsInOrganizations on gio.ProfileGroupID equals g.ProfileGroupID
                                      select gio).ToList();



                        foreach (var p in people)
                        {
                            p.AssignedTrainings = (from t in db.TrainingResults
                                                   where t.PersonID == p.Id
                                                   select new TrainingDto
                                                   {
                                                       Id = t.TrainingID,
                                                       StartDate = t.StartDate.Value,
                                                       EndDate = t.EndDate,
                                                       Result = t.Rating
                                                   }).ToList();

                            p.AssignedGroups = (from pg in db.PeopleInGroups
                                                where pg.PersonID == p.Id
                                                select pg).ToList();

                            if (p.AssignedGroups != null)
                            {
                                foreach (var grp in p.AssignedGroups)
                                {
                                    grp.GroupName = groups.FirstOrDefault(x => x.ProfileGroupID == grp.ProfileGroupID).Name;
                                }
                            }
                        }


                        obj.People = (from t in people
                                      where !t.IsDeleted
                                      select t).ToList();

                        obj.DeletedPeople = (from t in people
                                             where t.IsDeleted
                                             select t).ToList();

                        obj.Success = String.Empty;
                        break;
                    case BaseActionType.Delete:

                        obj.Current.DeleteUserID = obj.LoggedUser.Id;
                        obj.Current.DeletedDate = DateTime.Now;
                        db.Entry(obj.Current).State = EntityState.Modified;
                        LogService.InsertUserLogs(OperationLog.UserDelete, db, obj.Current.Id, obj.Current.DeleteUserID, obj.CurrentOrganization.OrganizationID);

                        db.SaveChanges();
                        obj.Success = "Dane usuniete!";
                        break;
                    case BaseActionType.Edit:


                        obj.Current.ModifiedUserID = obj.LoggedUser.Id;
                        db.Entry(obj.Current).State = EntityState.Modified;
                        LogService.InsertUserLogs(OperationLog.UserEdit, db, obj.Current.Id, obj.Current.ModifiedUserID, obj.CurrentOrganization.OrganizationID);

                        db.SaveChanges();
                        obj.Success = "Dane zapisane!";
                        break;

                    case BaseActionType.GetSimple:

                        obj.People = (from p in db.Users
                                      where p.OrganizationID == obj.CurrentOrganization.OrganizationID && p.Status == StatusEnum.Active
                                      && (p.Profile == ProfileEnum.User || p.Profile == ProfileEnum.Creator || p.Profile == ProfileEnum.Administrator || p.Profile == ProfileEnum.Manager)
                                      orderby p.RegistrationDate
                                      select p).ToList();
                        break;
                    default:
                        break;
                }

              
                return Request.CreateResponse(HttpStatusCode.Created, obj);
            }
            catch (Exception ex)
            {
                obj.ErrorMessage = ex.Message;
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, obj);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
