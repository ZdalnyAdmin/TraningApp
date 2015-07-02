using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DTO;
using AppEngine.Services;
using AppEngine.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                obj.LoggedUser = Person.GetLoggedPerson(User);
                if (obj.LoggedUser.Status == StatusEnum.Deleted)
                {
                    obj.ErrorMessage = "Uprawnienia uzytkownika wygasly!";
                    return Request.CreateResponse(HttpStatusCode.Created, obj);
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


                var take = 10;
                var skip = 0;


                switch (obj.ActionType)
                {
                    case BaseActionType.Get:
                        //todo logs for add and send invitation
                        obj.PeopleCount = (from p in db.Users
                                           where p.OrganizationID == obj.CurrentOrganization.OrganizationID &&
                                           (p.Status == StatusEnum.Active || p.Status == StatusEnum.Blocked)
                                           && (p.Profile == ProfileEnum.User || p.Profile == ProfileEnum.Creator || p.Profile == ProfileEnum.Administrator || p.Profile == ProfileEnum.Manager)
                                           orderby p.RegistrationDate
                                           select p).Count();


                        var people = (from p in db.Users
                                      where p.OrganizationID == obj.CurrentOrganization.OrganizationID &&
                                      (p.Status == StatusEnum.Active || p.Status == StatusEnum.Blocked)
                                      && (p.Profile == ProfileEnum.User || p.Profile == ProfileEnum.Creator || p.Profile == ProfileEnum.Administrator || p.Profile == ProfileEnum.Manager)
                                      orderby p.RegistrationDate
                                      select p).Take(20).ToList();

                        var deleted = (from p in db.Users
                                       where p.OrganizationID == obj.CurrentOrganization.OrganizationID &&
                                       (p.Status == StatusEnum.Deleted)
                                       && (p.Profile == ProfileEnum.User || p.Profile == ProfileEnum.Creator || p.Profile == ProfileEnum.Administrator || p.Profile == ProfileEnum.Manager)
                                       orderby p.RegistrationDate
                                       select p).Take(20).ToList();


                        var groups = (from gio in db.Groups
                                      join g in db.GroupsInOrganizations on gio.ProfileGroupID equals g.ProfileGroupID
                                      select gio).ToList();



                        foreach (var p in people)
                        {
                            p.AssignedTrainings = (from t in db.TrainingResults
                                                   join tr in db.Trainings on t.TrainingID equals tr.TrainingID
                                                   where t.PersonID == p.Id
                                                   orderby t.StartDate descending
                                                   select new TrainingDto
                                                   {
                                                       Id = t.TrainingID,
                                                       StartDate = t.StartDate.Value,
                                                       EndDate = t.EndDate,
                                                       Result = t.Rating,
                                                       Name = tr.Name
                                                   }).Take(5).ToList();

                            p.AssignedTrainingCount = (from t in db.TrainingResults
                                                       join tr in db.Trainings on t.TrainingID equals tr.TrainingID
                                                       where t.PersonID == p.Id
                                                       orderby t.StartDate descending
                                                       select t).Count();

                            var assignedGroups = (from pg in db.PeopleInGroups
                                                  where pg.PersonID == p.Id
                                                  select pg).ToList();

                            if (assignedGroups != null)
                            {
                                p.AssignedGroups = new List<ProfileGroup2Person>();
                                foreach (var grp in assignedGroups)
                                {
                                    p.AssignedGroups.Add(new ProfileGroup2Person()
                                    {
                                        ProfileGroupID = grp.ProfileGroupID,
                                        GroupName = groups.FirstOrDefault(x => x.ProfileGroupID == grp.ProfileGroupID).Name
                                    });
                                }
                            }
                        }


                        obj.People = people;

                        if (deleted != null && deleted.Any())
                        {

                            foreach (var p in deleted)
                            {
                                p.AssignedTrainings = (from t in db.TrainingResults
                                                        join tr in db.Trainings on t.TrainingID equals tr.TrainingID
                                                       where t.PersonID == p.Id
                                                       orderby t.StartDate descending
                                                       select new TrainingDto
                                                       {
                                                           Id = t.TrainingID,
                                                           StartDate = t.StartDate.Value,
                                                           EndDate = t.EndDate,
                                                           Result = t.Rating,
                                                           Name = tr.Name
                                                       }).ToList();

                                var assignedGroups = (from pg in db.PeopleInGroups
                                                      where pg.PersonID == p.Id
                                                      select pg).ToList();

                                if (assignedGroups != null)
                                {
                                    p.AssignedGroups = new List<ProfileGroup2Person>();
                                    foreach (var grp in assignedGroups)
                                    {
                                        p.AssignedGroups.Add(new ProfileGroup2Person()
                                        {
                                            ProfileGroupID = grp.ProfileGroupID,
                                            GroupName = groups.FirstOrDefault(x => x.ProfileGroupID == grp.ProfileGroupID).Name
                                        });
                                    }
                                }
                            }
                            obj.DeletedPeople = deleted;
                        }
                        obj.Success = String.Empty;
                        break;
                    case BaseActionType.Delete:

                        obj.Current.DeleteUserID = obj.LoggedUser.Id;
                        obj.Current.DeletedDate = DateTime.Now;
                        obj.Current.Status = StatusEnum.Deleted;

                        db.Entry(obj.Current).State = EntityState.Modified;
                        LogService.InsertUserLogs(OperationLog.UserDelete, db, obj.Current.Id, obj.Current.DeleteUserID, obj.CurrentOrganization.OrganizationID);

                        db.SaveChanges();

                        var current = obj.People.FirstOrDefault(x => x.Id == obj.Current.Id);
                        if (current != null)
                        {
                            obj.People.Remove(current);
                            obj.DeletedPeople.Add(obj.Current);
                        }

                        obj.Current = new Person();

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
                    case BaseActionType.GetSpecial:

                        take = 20;
                        skip = 0;

                        if (obj.People != null)
                        {
                            skip = obj.People.Count();
                        }


                        var collection = (from p in db.Users
                                          where p.OrganizationID == obj.CurrentOrganization.OrganizationID &&
                                          (p.Status == StatusEnum.Active || p.Status == StatusEnum.Blocked)
                                          && (p.Profile == ProfileEnum.User || p.Profile == ProfileEnum.Creator || p.Profile == ProfileEnum.Administrator || p.Profile == ProfileEnum.Manager)
                                          orderby p.RegistrationDate
                                          select p).Skip(skip).Take(take).ToList();

                        var groupsEx = (from gio in db.Groups
                                        join g in db.GroupsInOrganizations on gio.ProfileGroupID equals g.ProfileGroupID
                                        select gio).ToList();



                        foreach (var p in collection)
                        {
                            p.AssignedTrainings = (from t in db.TrainingResults
                                                    join tr in db.Trainings on t.TrainingID equals tr.TrainingID
                                                   where t.PersonID == p.Id
                                                   orderby t.StartDate descending
                                                   select new TrainingDto
                                                   {
                                                       Id = t.TrainingID,
                                                       StartDate = t.StartDate.Value,
                                                       EndDate = t.EndDate,
                                                       Result = t.Rating,
                                                       Name = tr.Name
                                                   }).ToList();

                            var assignedGroups = (from pg in db.PeopleInGroups
                                                  where pg.PersonID == p.Id
                                                  select pg).ToList();

                            if (assignedGroups != null)
                            {
                                p.AssignedGroups = new List<ProfileGroup2Person>();
                                foreach (var grp in assignedGroups)
                                {
                                    p.AssignedGroups.Add(new ProfileGroup2Person()
                                    {
                                        ProfileGroupID = grp.ProfileGroupID,
                                        GroupName = groupsEx.FirstOrDefault(x => x.ProfileGroupID == grp.ProfileGroupID).Name
                                    });
                                }
                            }
                        }


                        obj.People.AddRange(collection);

                        obj.Success = "";

                        break;

                    case BaseActionType.GetExtData:

                        if (obj.Current == null)
                        {
                            obj.ErrorMessage = "Nie wybrano uzytkowanika";
                            return Request.CreateResponse(HttpStatusCode.Created, obj);
                        }

                        take = 5;
                        skip = 0;

                        if (obj.Current.AssignedTrainings != null)
                        {
                            skip = obj.Current.AssignedTrainings.Count();
                        }
                        else
                        {
                            obj.Current.AssignedTrainings = new List<TrainingDto>();
                        }


                        var trainingsEx = (from t in db.TrainingResults
                                            join tr in db.Trainings on t.TrainingID equals tr.TrainingID
                                           where t.PersonID == obj.Current.Id
                                           orderby t.StartDate descending
                                           select new TrainingDto
                                           {
                                               Id = t.TrainingID,
                                               StartDate = t.StartDate.Value,
                                               EndDate = t.EndDate,
                                               Result = t.Rating,
                                               Name = tr.Name
                                           }).Skip(skip).Take(take);

                        if (trainingsEx != null && trainingsEx.Any())
                        {
                            obj.Current.AssignedTrainings.AddRange(trainingsEx.ToList());
                        }

                        obj.Success = "";

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
