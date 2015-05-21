using AppEngine.Helpers;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
using AppEngine.Models.DTO;
using AppEngine.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace AppEngine.Services
{
    public class TrainingService
    {
        public static bool ManageTrainings(EFContext context, TrainingViewModel model, bool isInternal)
        {
            try
            {
                model.ErrorMessage = String.Empty;

                model.Success = String.Empty;

                if (isInternal)
                {
                    if (model.CurrentOrganization == null)
                    {
                        model.CurrentOrganization = context.Organizations.FirstOrDefault(x => x.OrganizationID == model.LoggedUser.OrganizationID);
                    }

                    if (model.CurrentOrganization == null)
                    {
                        model.ErrorMessage = "Brak organizacji do ktorej mozna przypisac grupe!";
                        return true;
                    }
                }

                switch (model.ActionType)
                {
                    case AppEngine.Models.DataBusiness.BaseActionType.Get:

                        break;
                    case AppEngine.Models.DataBusiness.BaseActionType.Delete:
                        model.Current.IsDeleted = false;
                        model.Current.DeletedUserID = model.LoggedUser.Id;
                        model.Current.DeletedDate = DateTime.Now;
                        context.Entry(model.Current).State = EntityState.Modified;

                        var git = context.TrainingInGroups.Where(x => x.TrainingID == model.Current.TrainingID).ToList();
                        if (git != null)
                        {
                            context.TrainingInGroups.RemoveRange(git);
                        }

                        var tio = context.TrainingsInOrganizations.Where(x => x.TrainingID == model.Current.TrainingID).ToList();
                        if (tio != null)
                        {
                            context.TrainingsInOrganizations.RemoveRange(tio);
                        }


                        context.SaveChanges();

                        model.Success = "Usuniecie szkolenia zakonczylo sie sukcesem!";

                        break;
                    case AppEngine.Models.DataBusiness.BaseActionType.Edit:

                        model.Current.ModifieddUserID = model.LoggedUser.Id;
                        model.Current.ModifiedDate = DateTime.Now;

                        context.Entry(model.Current).State = EntityState.Modified;

                        //todo assigned group

                        if (isInternal)
                        {
                            //todo change to only update
                            if (model.Current.Groups != null && model.Current.Groups.Any())
                            {
                                foreach (var item in model.Current.Groups)
                                {
                                    var grp = new ProfileGroup2Trainings();
                                    grp.IsDeleted = false;
                                    grp.ProfileGroupID = item.ProfileGroupID;
                                    grp.TrainingID = model.Current.TrainingID;
                                    context.TrainingInGroups.Remove(grp);
                                }
                            }


                            if (model.Groups != null && model.Groups.Any())
                            {
                                foreach (var item in model.Groups)
                                {
                                    var grp = new ProfileGroup2Trainings();
                                    grp.IsDeleted = false;
                                    grp.ProfileGroupID = item.ProfileGroupID;
                                    grp.TrainingID = model.Current.TrainingID;
                                    context.TrainingInGroups.Add(grp);
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in model.Current.Organizations)
                            {
                                var trainingInOrganization = new Trainings2Organizations();
                                trainingInOrganization.Organization = item;
                                trainingInOrganization.OrganizationID = item.OrganizationID;
                                trainingInOrganization.Training = model.Current;
                                trainingInOrganization.TrainingID = model.Current.TrainingID;
                                trainingInOrganization.IsDeleted = false;
                                context.TrainingsInOrganizations.Remove(trainingInOrganization);
                            }


                            if (model.Organizations == null || model.Organizations.Any())
                            {
                                model.Organizations = new List<Organization>();
                                model.Organizations = context.Organizations.Where(x => !x.IsDeleted).ToList();
                            }


                            var organizations = new List<Trainings2Organizations>();


                            foreach (var item in model.Organizations)
                            {
                                var trainingInOrganization = new Trainings2Organizations();
                                trainingInOrganization.Organization = item;
                                trainingInOrganization.OrganizationID = item.OrganizationID;
                                trainingInOrganization.Training = model.Current;
                                trainingInOrganization.TrainingID = model.Current.TrainingID;
                                trainingInOrganization.IsDeleted = false;
                                context.TrainingsInOrganizations.Add(trainingInOrganization);
                            }

                            if (organizations.Any())
                            {
                                context.TrainingsInOrganizations.AddRange(organizations);
                            }
                        }


                        context.SaveChanges();

                        if (isInternal)
                        {
                            LogService.InsertTrainingLogs(OperationLog.TrainingEdit, context, model.Current.TrainingID, model.LoggedUser.Id);
                        }

                        model.Success = "Edycja szkolenia zakonczylo sie sukcesem!";

                        break;
                    case AppEngine.Models.DataBusiness.BaseActionType.Add:

                        var serverPath = AppSettings.ServerPath();
                        var path = Path.Combine(HttpRuntime.AppDomainAppPath, serverPath);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }


                        model.Current.CreateDate = DateTime.Now;
                        model.Current.CreateUserID = model.LoggedUser.Id;

                        model.Current.IsDeleted = false;
                        model.Current.IsActive = true;
                        if (String.IsNullOrEmpty(model.Current.TrainingResources))
                        {
                            model.Current.TrainingResources = @"Assets\Image\main_image.png";
                        }

                        var size = 0m;
                        model.Current.TrainingResources = AppSettings.CopyFile(path, HttpRuntime.AppDomainAppPath, model.Current.TrainingResources, out size);
                        if (!String.IsNullOrEmpty(model.Current.PassResources))
                        {
                            size = 0m;
                            model.Current.PassResources = AppSettings.CopyFile(path, HttpRuntime.AppDomainAppPath, model.Current.PassResources, out size);
                        }


                        model.Current.TrainingType = isInternal ? TrainingType.Internal : TrainingType.Kenpro;
                        int index = 0;

                        if (model.Details != null && model.Details.Any())
                        {
                            foreach (var item in model.Details)
                            {
                                item.DisplayNo = index;
                                index++;

                                //move file and save size in MG
                                if (!String.IsNullOrEmpty(item.InternalResource))
                                {
                                    size = 0m;
                                    item.InternalResource = AppSettings.CopyFile(path, HttpRuntime.AppDomainAppPath, item.InternalResource, out size);
                                    item.FileSize = size;
                                }
                            }
                            model.Current.Details = model.Details;
                        }

                        index = 0;
                        if (model.Questions != null && model.Questions.Any())
                        {
                            foreach (var item in model.Questions)
                            {
                                item.DisplayNo = index;
                                index++;
                            }
                            model.Current.Questions = model.Questions;
                        }


                        context.Trainings.Add(model.Current);
                        context.SaveChanges();

                        if (isInternal)
                        {
                            LogService.InsertTrainingLogs(OperationLog.TrainingCreate, context, model.Current.TrainingID, model.LoggedUser.Id);
                            if (model.Current.Groups != null && model.Current.Groups.Any())
                            {
                                foreach (var item in model.Current.Groups)
                                {
                                    var grp = new ProfileGroup2Trainings();
                                    grp.IsDeleted = false;
                                    grp.ProfileGroupID = item.ProfileGroupID;
                                    grp.TrainingID = model.Current.TrainingID;
                                    context.TrainingInGroups.Add(grp);
                                }
                                context.SaveChanges();
                            }


                            var trainingInOrganization = new Trainings2Organizations();
                            trainingInOrganization.OrganizationID = model.CurrentOrganization.OrganizationID;
                            trainingInOrganization.Training = model.Current;
                            trainingInOrganization.TrainingID = model.Current.TrainingID;
                            trainingInOrganization.IsDeleted = false;
                            context.TrainingsInOrganizations.Add(trainingInOrganization);
                            context.SaveChanges();

                        }
                        else
                        {
                            if (model.Organizations == null || model.Organizations.Any())
                            {
                                model.Organizations = new List<Organization>();
                                model.Organizations = context.Organizations.Where(x => !x.IsDeleted).ToList();
                            }


                            var organizations = new List<Trainings2Organizations>();


                            foreach (var item in model.Organizations)
                            {
                                var trainingInOrganization = new Trainings2Organizations();
                                trainingInOrganization.Organization = item;
                                trainingInOrganization.OrganizationID = item.OrganizationID;
                                trainingInOrganization.Training = model.Current;
                                trainingInOrganization.TrainingID = model.Current.TrainingID;
                                trainingInOrganization.IsDeleted = false;
                                context.TrainingsInOrganizations.Add(trainingInOrganization);
                            }

                            if (organizations.Any())
                            {
                                context.TrainingsInOrganizations.AddRange(organizations);
                                context.SaveChanges();
                            }
                        }



                        model.Current = new Training();
                        model.Current.PassResult = 80;
                        model.Details = new List<TrainingDetail>();
                        model.Questions = new List<TrainingQuestion>();
                        model.Organizations = new List<Organization>();

                        if (isInternal)
                        {
                            model.Groups = model.Groups = (from gio in context.GroupsInOrganizations
                                                           join g in context.Groups on gio.ProfileGroupID equals g.ProfileGroupID
                                                           where gio.OrganizationID == model.CurrentOrganization.OrganizationID && !g.IsDeleted
                                                           select g).ToList();
                        }

                        model.Success = "Dodanie szkolenia zakonczylo sie sukcesem!";

                        break;
                    case BaseActionType.GetSimple:

                        if (isInternal)
                        {
                            model.Trainings = (from t in context.Trainings
                                               join to in context.TrainingsInOrganizations on t.TrainingID equals to.TrainingID
                                               where to.OrganizationID == model.CurrentOrganization.OrganizationID && t.TrainingType == TrainingType.Internal
                                               orderby t.CreateDate
                                               select t).ToList();

                            foreach (var item in model.Trainings)
                            {
                                item.AssignedGroups = (from git1 in context.TrainingInGroups
                                                       join g1 in context.Groups on git1.ProfileGroupID equals g1.ProfileGroupID
                                                       where git1.TrainingID == item.TrainingID
                                                       select new CommonDto
                                                       {
                                                           Name = g1.Name,
                                                           Id = g1.ProfileGroupID
                                                       }).ToList();

                                if (!String.IsNullOrEmpty(item.CreateUserID))
                                {
                                    item.UserName = context.Users.FirstOrDefault(x => x.Id == item.CreateUserID).UserName;
                                }
                            }

                        }
                        else
                        {
                            model.Trainings = (from ext in context.Trainings
                                               where ext.TrainingType == TrainingType.Kenpro
                                               orderby ext.CreateDate
                                               select ext).ToList();



                            foreach (var item in model.Trainings)
                            {
                                if (!String.IsNullOrEmpty(item.CreateUserID))
                                {
                                    item.UserName = context.Users.FirstOrDefault(x => x.Id == item.CreateUserID).UserName;
                                }

                            }
                        }

                        model.Success = String.Empty;

                        break;
                    case BaseActionType.GetExtData:


                        model.Current = new Training();
                        model.Current.PassResult = 80;
                        model.Details = new List<TrainingDetail>();
                        model.Questions = new List<TrainingQuestion>();
                        model.AvailableForAll = true;

                        if (isInternal)
                        {
                            model.Groups = (from gio in context.GroupsInOrganizations
                                            join g in context.Groups on gio.ProfileGroupID equals g.ProfileGroupID
                                            where gio.OrganizationID == model.CurrentOrganization.OrganizationID && !g.IsDeleted
                                            select g).ToList();
                        }

                        model.Success = String.Empty;

                        break;

                    case BaseActionType.ById:

                        if (model.trainingID == 0)
                        {
                            model.ErrorMessage = "Brak id szkolenia.";
                            return true;
                        }

                        model.Current = context.Trainings.FirstOrDefault(x => x.TrainingID == model.trainingID);

                        if (model.Current == null)
                        {
                            model.ErrorMessage = "Blad wczytanie szkolenia";
                        }

                        model.Current.Details = context.TrainingDetails.Where(x => x.TrainingID == model.trainingID).ToList();
                        model.Current.Questions = context.TrainingQuestons.Where(x => x.TrainingID == model.trainingID).ToList();

                        foreach(var question in model.Current.Questions)
                        {
                            question.Answers = context.TrainingAnswers.Where(x => x.TrainingQuestionID == question.TrainingQuestionID).ToList();
                        }

                        if (isInternal)
                        {
                            //todo groups
                            model.Groups = (from extTig in context.TrainingInGroups
                                            join extG in context.Groups on extTig.ProfileGroupID equals extG.ProfileGroupID
                                            where extTig.TrainingID == model.Current.TrainingID
                                            select extG).ToList();
                            model.Current.Groups = model.Groups;
                        }
                        else
                        {
                            model.Organizations = (from extTio in context.TrainingsInOrganizations
                                                   join extO in context.Organizations on extTio.OrganizationID equals extO.OrganizationID
                                                   where extTio.TrainingID == model.Current.TrainingID
                                                   select extO).ToList();
                            model.Current.Organizations = model.Organizations;
                        }

                        model.Success = String.Empty;

                        break;

                    case BaseActionType.GetSpecial:

                        var take = 50;
                        var skip = 0;

                        if (model.InternalTrainings != null)
                        {
                            skip = model.InternalTrainings.Count();
                        }
                        else
                        {
                            model.InternalTrainings = new List<TrainingDto>();
                        }

                        var query = (from t in context.Trainings
                                     join to in context.TrainingsInOrganizations on t.TrainingID equals to.TrainingID
                                     join o in context.Organizations on to.OrganizationID equals o.OrganizationID
                                     where t.TrainingType == TrainingType.Internal
                                     orderby t.CreateDate
                                     select new TrainingDto
                                        {
                                            Id = t.TrainingID,
                                            StartDate = t.CreateDate,
                                            Name = t.Name,
                                            Organization = o.Name,
                                            CreatorID = t.CreateUserID
                                        }).Skip(skip).Take(take);


                        var collection = query.ToList();

                        foreach (var item in collection)
                        {
                            var result = (from tr in context.Logs
                                          where tr.TrainingID == item.Id && !tr.IsSystem && tr.OperationType == OperationLog.TrainingEdit
                                          orderby tr.ModifiedDate
                                          select tr).FirstOrDefault();

                            if (result != null)
                            {
                                item.EndDate = result.ModifiedDate;
                            }

                            if (!String.IsNullOrEmpty(item.CreatorID))
                            {
                                item.CreatorName = context.Users.FirstOrDefault(x => x.Id == item.CreatorID).UserName;
                            }
                        }

                        model.Success = String.Empty;

                        model.InternalTrainings.AddRange(collection);

                        break;
                    default:
                        break;
                }



                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
