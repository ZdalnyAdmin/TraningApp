using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
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
                if (obj.LoggedUser == null)
                {
                    obj.LoggedUser = Person.GetLoggedPerson(User);
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
                            var protector = db.Users.FirstOrDefault(x => x.Profile == ProfileEnum.Administrator && x.OrganizationID == obj.LoggedUser.OrganizationID);

                            AppSetting setting = null;
                            if (protector != null)
                            {
                                setting = db.AppSettings.FirstOrDefault(x => x.ProtectorID == protector.Id);
                            }

                            obj.ShowExternal = setting != null ? setting.IsTrainingAvailableForAll : false;
                        }

                        obj.InternalTrainings = (from t in db.Trainings
                                                     join to in db.TrainingsInOrganizations on t.TrainingID equals to.TrainingID
                                                 where to.OrganizationID == obj.LoggedUser.OrganizationID && t.TrainingType == TrainingType.Internal
                                                     select t).ToList();



                        //obj.InternalTrainings = (from t in db.Trainings

                        //                         select t).ToList();

                        foreach (var training in obj.InternalTrainings)
                        {
                            training.Logs = (from item in db.Logs
                                             join user in db.Users on item.ModifiedUserID equals user.Id
                                             where item.TrainingID == training.TrainingID && !item.IsSystemLog &&
                                             (item.OperationType == OperationLog.TrainingCreate || item.OperationType == OperationLog.TrainingEdit)
                                             select new SimpleObject
                                             {
                                                 Date = item.ModifiedDate.Value,
                                                 Name = user.UserName
                                             }).ToList();

                            training.AssignedGroups = (from item in db.TrainingInGroups
                                                       join grp in db.Groups on item.ProfileGroupID equals grp.ProfileGroupID
                                                       where grp.Name != "Wszyscy"
                                                       select new SimpleObject
                                                       {
                                                           Name = grp.Name
                                                       }).ToList();
                        }


                        break;
                    case TrainingManagmentActionType.GetExternal:

                        obj.InternalTrainings = (from t in db.Trainings
                                                 where t.TrainingType == TrainingType.Kenpro
                                                 select t).ToList();

                        foreach (var training in obj.InternalTrainings)
                        {
                            training.Logs = (from item in db.Logs
                                             join user in db.Users on item.ModifiedUserID equals user.Id
                                             where item.TrainingID == training.TrainingID && !item.IsSystemLog &&
                                             (item.OperationType == OperationLog.TrainingCreate || item.OperationType == OperationLog.TrainingEdit)
                                             select new SimpleObject
                                             {
                                                 Date = item.ModifiedDate.Value,
                                                 Name = user.UserName
                                             }).ToList();

                            training.AssignedGroups = (from item in db.GroupsInOrganizations
                                                       join grp in db.Groups on item.ProfileGroupID equals grp.ProfileGroupID
                                                       where grp.Name != "Wszyscy"
                                                       select new SimpleObject
                                                       {
                                                           Name = grp.Name
                                                       }).ToList();
                        }

                        break;
                    case TrainingManagmentActionType.GetSettings:




                        break;
                    case TrainingManagmentActionType.GetGroups:

                        obj.Groups = (from grp in db.GroupsInOrganizations
                                      join g in db.Groups on grp.ProfileGroupID equals g.ProfileGroupID
                                      where grp.OrganizationID == obj.CurrentOrganization.OrganizationID && g.Name != "Wszyscy"
                                      select new ProfileGroup
                                      {
                                          ProfileGroupID = g.ProfileGroupID,
                                          Name = g.Name
                                      }
                                     ).ToList();

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

                LogService.InsertTrainingLogs(OperationLog.TrainingEdit, db, obj.Current.TrainingID, obj.LoggedUser.Id);
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
