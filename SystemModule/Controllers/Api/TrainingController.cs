using AppEngine.Helpers;
using AppEngine.Models;
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
    public class TrainingController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Training> Get()
        {
            //get from correct profile
            var list = db.Trainings.ToList();
            if (list == null)
            {
                return null;
            }
            foreach (var item in list)
            {
                var user = db.Users.FirstOrDefault(x => x.Id == item.CreateUserID);
                if (user == null)
                {
                    continue;
                }
                item.SetCreateUserName(user.DisplayName);
                var runCounter = db.TrainingResults.Count(x => x.TrainingID == item.TrainingID);
                item.SetRunTrainingStats(runCounter);

                var groups = (from pg in db.TrainingInGroups
                              join g in db.Groups on pg.ProfileGroupID equals g.ProfileGroupID
                              where pg.TrainingID == item.TrainingID
                              select g.Name).ToList();
                item.SetAssignedGroups(groups);
            }

            return list;
        }

        public HttpResponseMessage Post(Training obj)
        {
            try
            {
                if (obj.TrainingID != 0)
                {
                    //hak
                    if (obj.TrainingID == -1)
                    {
                        var training = db.Trainings.FirstOrDefault();
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, training);
                        return response;
                    }
                    else
                    {
                        var training = db.Trainings.FirstOrDefault(x => x.TrainingID == obj.TrainingID);
                        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, training);
                        return response;
                    }
                }
                obj.CreateDate = DateTime.Now;
                var usr = Person.GetLoggedPerson(User);
                obj.CreateUserID = usr.Id;
                obj.IsDeleted = false;
                obj.IsActive = true;
                obj.TrainingType = TrainingType.Kenpro;
                int index = 0;
                if (obj.Details != null && obj.Details.Any())
                {
                    foreach (var item in obj.Details)
                    {
                        item.DisplayNo = index;
                        index++;
                    }
                }

                index = 0;
                if (obj.Questions != null && obj.Questions.Any())
                {
                    foreach (var item in obj.Questions)
                    {
                        item.DisplayNo = index;
                        index++;
                    }
                }

                if (ModelState.IsValid)
                {
                    db.Trainings.Add(obj);
                    db.SaveChanges();
                    LogService.InsertTrainingLogs(OperationLog.TrainingCreate, db, obj.TrainingID, obj.CreateUserID);
                    if (obj.Groups != null && obj.Groups.Any())
                    {
                        foreach (var item in obj.Groups)
                        {
                            var grp = new ProfileGroup2Trainings();
                            grp.IsDeleted = false;
                            grp.ProfileGroupID = item.ProfileGroupID;
                            grp.TrainingID = obj.TrainingID;
                            db.TrainingInGroups.Add(grp);
                        }
                        db.SaveChanges();
                    }
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
        public HttpResponseMessage Put(Training obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            if (obj.IsDeleted)
            {
                var usr = Person.GetLoggedPerson(User);
                obj.DeletedUserID = Helpers.GetUserID(usr);
                obj.DeletedDate = DateTime.Now;
            }

            db.Entry(obj).State = EntityState.Modified;

            try
            {
                var assignedGroup = (from t in db.TrainingInGroups
                                     where t.TrainingID == obj.TrainingID
                                     select t).ToList();

                foreach (var item in obj.Groups)
                {
                    var temp = assignedGroup.FirstOrDefault(x => x.ProfileGroupID == item.ProfileGroupID);
                    if (temp == null)
                    {
                        var grp = new ProfileGroup2Trainings();
                        grp.IsDeleted = false;
                        grp.ProfileGroupID = item.ProfileGroupID;
                        grp.TrainingID = obj.TrainingID;
                        db.TrainingInGroups.Add(grp);
                    }
                }

                foreach (var item in assignedGroup)
                {
                    var temp = obj.Groups.FirstOrDefault(x => x.ProfileGroupID == item.ProfileGroupID);
                    if (temp == null)
                    {
                        db.TrainingInGroups.Remove(item);
                    }
                }
                db.SaveChanges();
                LogService.InsertTrainingLogs(OperationLog.TrainingEdit, db, obj.TrainingID, obj.CreateUserID);
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
