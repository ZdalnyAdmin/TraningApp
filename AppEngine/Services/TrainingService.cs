using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
using AppEngine.Models.DTO;
using AppEngine.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AppEngine.Services
{
    public class TrainingService
    {
        public static bool ManageTrainings(EFContext context, TrainingViewModel model, bool isInternal)
        {
            try
            {
                model.ErrorMessage = String.Empty;

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


                        break;
                    case AppEngine.Models.DataBusiness.BaseActionType.Edit:

                        model.Current.ModifieddUserID = model.LoggedUser.Id;
                        model.Current.ModifiedDate = DateTime.Now;

                        context.Entry(model.Current).State = EntityState.Modified;

                        //todo assigned group


                        context.SaveChanges();

                        if (isInternal)
                        {
                            LogService.InsertTrainingLogs(OperationLog.TrainingEdit, context, model.Current.TrainingID, model.LoggedUser.Id);

                        }

                        break;
                    case AppEngine.Models.DataBusiness.BaseActionType.Add:

                        model.Current.CreateDate = DateTime.Now;
                        model.Current.CreateUserID = model.LoggedUser.Id;

                        model.Current.IsDeleted = false;
                        model.Current.IsActive = false;

                        model.Current.TrainingType = isInternal ? TrainingType.Internal : TrainingType.Kenpro;
                        int index = 0;

                        if (model.Details != null && model.Details.Any())
                        {
                            foreach (var item in model.Details)
                            {
                                item.DisplayNo = index;
                                index++;
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
                                context.SaveChanges();
                            }

                            var organization = context.Organizations.FirstOrDefault(x => x.ProtectorID == model.LoggedUser.Id);
                            if (organization != null)
                            {
                                var trainingInOrganization = new Trainings2Organizations();
                                trainingInOrganization.Organization = organization;
                                trainingInOrganization.OrganizationID = organization.OrganizationID;
                                trainingInOrganization.Training = model.Current;
                                trainingInOrganization.TrainingID = model.Current.TrainingID;
                                trainingInOrganization.IsDeleted = false;
                                context.TrainingsInOrganizations.Add(trainingInOrganization);
                                context.SaveChanges();
                            }
                        }


                        model.Current = new Training();
                        model.Current.PassResult = 80;
                        model.Details = new List<TrainingDetail>();
                        model.Questions = new List<TrainingQuestion>();

                        if (isInternal)
                        {
                            model.Groups = model.Groups = (from gio in context.GroupsInOrganizations
                                                           join g in context.Groups on gio.ProfileGroupID equals g.ProfileGroupID
                                                           where gio.OrganizationID == model.CurrentOrganization.OrganizationID && !g.IsDeleted
                                                           select new ProfileGroup
                                                           {
                                                               Name = g.Name,
                                                               ProfileGroupID = g.ProfileGroupID
                                                           }).ToList();
                        }

                        break;
                    case BaseActionType.GetSimple:

                        if (isInternal)
                        {
                            model.Trainings = (from t in context.Trainings
                                               join to in context.TrainingsInOrganizations on t.TrainingID equals to.TrainingID
                                               where to.OrganizationID == model.CurrentOrganization.OrganizationID && t.TrainingType == TrainingType.Internal
                                               orderby t.CreateDate
                                               select new Training
                                                  {
                                                      TrainingID = t.TrainingID,
                                                      CreateDate = t.CreateDate,
                                                      Name = t.Name
                                                  }).ToList();

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


                        break;
                    case BaseActionType.GetExtData:


                        model.Current = new Training();
                        model.Current.PassResult = 80;
                        model.Details = new List<TrainingDetail>();
                        model.Questions = new List<TrainingQuestion>();

                        if (isInternal)
                        {
                            model.Groups = (from gio in context.GroupsInOrganizations
                                            join g in context.Groups on gio.ProfileGroupID equals g.ProfileGroupID
                                            where gio.OrganizationID == model.CurrentOrganization.OrganizationID && !g.IsDeleted
                                            select new ProfileGroup
                                            {
                                                Name = g.Name,
                                                ProfileGroupID = g.ProfileGroupID
                                            }).ToList();
                        }
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
