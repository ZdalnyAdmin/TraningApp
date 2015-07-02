using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
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

namespace OrganizationModule.Controllers.Api
{
    public class TrainingManagmentController : ApiController
    {
        private EFContext db = new EFContext();

        public HttpResponseMessage Post(TrainingManagmentViewModel obj)
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

                switch (obj.ActionType)
                {
                    case TrainingManagmentActionType.GetInternal:

                        obj.ShowExternal = false;
                        if (obj.LoggedUser.Profile == ProfileEnum.Protector)
                        {
                            obj.ShowExternal = true;
                        }
                        else if (obj.LoggedUser.Profile == ProfileEnum.Administrator)
                        {
                            obj.ShowExternal = false;
                            //get protector
                            if(obj.CurrentOrganization != null)
                            {
                                obj.ShowExternal = obj.CurrentOrganization.IsGlobalAvailable;
                            }
                        }

                        obj.InternalTrainings = (from t in db.Trainings
                                                 join to in db.TrainingsInOrganizations on t.TrainingID equals to.TrainingID
                                                 where to.OrganizationID == obj.LoggedUser.OrganizationID && t.TrainingType == TrainingType.Internal
                                                 select t).ToList();

                        foreach (var training in obj.InternalTrainings)
                        {
                            training.Logs = (from item in db.Logs
                                             join user in db.Users on item.ModifiedUserID equals user.Id
                                             where item.TrainingID == training.TrainingID && !item.IsSystemLog &&
                                             (item.OperationType == OperationLog.TrainingCreate || item.OperationType == OperationLog.TrainingEdit)
                                             select new CommonDto
                                             {
                                                 Date = item.ModifiedDate.Value,
                                                 Name = user.UserName
                                             }).ToList();

                            training.AssignedGroups = (from item in db.TrainingInGroups
                                                       join grp in db.Groups on item.ProfileGroupID equals grp.ProfileGroupID
                                                       where item.TrainingID == training.TrainingID
                                                       select new CommonDto
                                                       {
                                                           Name = grp.Name
                                                       }).ToList();

                            var createdUser = db.Users.FirstOrDefault(x => x.Id == training.CreateUserID);
                            if(createdUser != null)
                            {
                                training.UserName = createdUser.DisplayName;
                            }

                        }

                        obj.Success = String.Empty;

                        break;
                    case TrainingManagmentActionType.GetExternal:

                        obj.ExternalTrainings = (from t in db.Trainings
                                                 where t.TrainingType == TrainingType.Kenpro
                                                 select t).ToList();


                        var groups = (from grp in db.GroupsInOrganizations
                                      join g in db.Groups on grp.ProfileGroupID equals g.ProfileGroupID
                                      where grp.OrganizationID == obj.CurrentOrganization.OrganizationID
                                      select new
                                      {

                                          ProfileGroupID = g.ProfileGroupID,
                                          Name = g.Name
                                      }
                             ).ToList();

                        obj.Groups = new List<ProfileGroup>();

                        if (groups.Any())
                        {
                            obj.Groups = (from grp in groups
                                          group grp by grp.ProfileGroupID
                                              into gp
                                              select new ProfileGroup
                                              {
                                                  ProfileGroupID = gp.Key,
                                                  Name = groups.FirstOrDefault(x => x.ProfileGroupID == gp.Key).Name
                                              }).ToList();
                        }

                        if (obj.ExternalTrainings != null)
                        {
                            foreach (var training in obj.ExternalTrainings)
                            {
                                training.AssignedGroups = (from item in db.TrainingInGroups
                                                           join grp in db.Groups on item.ProfileGroupID equals grp.ProfileGroupID
                                                           where item.TrainingID == training.TrainingID
                                                           select new CommonDto
                                                           {
                                                               Name = grp.Name,
                                                               Id = grp.ProfileGroupID
                                                           }).ToList();
                            }
                        }

                        obj.Success = String.Empty;

                        break;
                    case TrainingManagmentActionType.GetSettings:




                        break;
                    case TrainingManagmentActionType.SaveGroups:

                        if (obj.Current == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Grupa nie istnieje");
                        }


                        var assigend = (from item in db.TrainingInGroups
                                        join grp in db.Groups on item.ProfileGroupID equals grp.ProfileGroupID
                                        where  item.TrainingID == obj.Current.TrainingID
                                        select item).ToList();

                        if (assigend != null && assigend.Any())
                        {
                            db.TrainingInGroups.RemoveRange(assigend);
                            db.SaveChanges();
                        }

                        if (obj.Current.Groups.Any())
                        {
                            foreach (var item in obj.Current.Groups)
                            {
                                var tig = new ProfileGroup2Trainings();
                                tig.TrainingID = obj.Current.TrainingID;
                                tig.ProfileGroupID = item.ProfileGroupID;
                                tig.IsDeleted = false;
                                db.TrainingInGroups.Add(tig);
                            }

                            db.SaveChanges();
                            obj.Current.AssignedGroups = (from item in db.TrainingInGroups
                                                          join grp in db.Groups on item.ProfileGroupID equals grp.ProfileGroupID
                                                          where  item.TrainingID == obj.Current.TrainingID
                                                          select new CommonDto
                                                          {
                                                              Name = grp.Name,
                                                              Id = grp.ProfileGroupID
                                                          }).ToList();
                        }

                        var current = obj.ExternalTrainings.FirstOrDefault(x => x.TrainingID == obj.Current.TrainingID);
                        obj.ExternalTrainings.Remove(current);

                        obj.ExternalTrainings.Add(obj.Current);

                        obj.ExternalTrainings = obj.ExternalTrainings.OrderBy(x => x.CreateDate).ToList();

                        obj.Current = new Training();

                        obj.Success = "Dane zapisane!";

                        break;
                    default:
                        break;
                }

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, obj);
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(TrainingManagmentViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (obj.Current.IsDeleted)
            {
                obj.Current.DeletedUserID = obj.LoggedUser.Id;
                obj.Current.DeletedDate = DateTime.Now;
            }
            else
            {
                obj.Current.ModifieddUserID = obj.LoggedUser.Id;
                obj.Current.ModifiedDate = DateTime.Now;
            }

            db.Entry(obj.Current).State = EntityState.Modified;

            try
            {

                db.SaveChanges();

                LogService.InsertTrainingLogs(OperationLog.TrainingEdit, db, obj.Current.TrainingID, obj.LoggedUser.Id, obj.CurrentOrganization != null ? obj.CurrentOrganization.OrganizationID : 0);
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
