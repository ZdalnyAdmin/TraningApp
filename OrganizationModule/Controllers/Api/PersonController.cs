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
        public HttpResponseMessage Post(UserManagmentViewModel obj)
        {
            if (ModelState.IsValid)
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
                        return Request.CreateResponse(HttpStatusCode.Created, obj); ;
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




                            foreach (var p in people)
                            {
                                p.AssignedTrainings = (from t in db.TrainingResults
                                                       where t.PersonID == p.Id
                                                       select new TrainingDto
                                                       {
                                                          Id= t.TrainingID,
                                                          StartDate = t.StartDate.Value,
                                                          EndDate = t.EndDate,
                                                          Result = t.Rating
                                                       }).ToList();

                                p.AssignedGroups = (from pg in db.PeopleInGroups
                                                    join g in db.Groups on pg.ProfileGroupID equals g.ProfileGroupID
                                                    where pg.PersonID == p.Id
                                                    select new ProfileGroup2Person
                                                    {
                                                        ProfileGroupID = pg.ProfileGroupID,
                                                        PersonID = pg.PersonID,
                                                        ProfileGroup2PersonID = pg.ProfileGroup2PersonID,
                                                        GroupName = g.Name,
                                                        IsDeleted = false
                                                    }).ToList();


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
                            LogService.InsertUserLogs(OperationLog.UserDelete, db, obj.Current.Id, obj.Current.DeleteUserID);

                            db.SaveChanges();
                            obj.Success = "Dane usuniete!";
                            break;
                        case BaseActionType.Edit:


                            obj.Current.ModifiedUserID = obj.LoggedUser.Id;
                            db.Entry(obj.Current).State = EntityState.Modified;
                            LogService.InsertUserLogs(OperationLog.UserEdit, db, obj.Current.Id, obj.Current.ModifiedUserID);

                            db.SaveChanges();
                            obj.Success = "Dane zapisane!";
                            break;

                        case BaseActionType.GetSimple:

                            obj.People = (from p in db.Users
                                          where p.OrganizationID == obj.CurrentOrganization.OrganizationID && p.Status == StatusEnum.Active
                                          && (p.Profile == ProfileEnum.User || p.Profile == ProfileEnum.Creator || p.Profile == ProfileEnum.Administrator || p.Profile == ProfileEnum.Manager)
                                          orderby p.RegistrationDate
                                          select p).ToList();
                            obj.Success = "Dane zapisane!";
                            break;
                        default:
                            break;
                    }


                }
                catch (Exception ex)
                {

                }



                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, obj);
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
