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

                var serverPath = String.Empty;
                var path = String.Empty;
                var fileSize = 0m;
                var index = 0;
                var availableSpace = 0m;

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

                    availableSpace = model.CurrentOrganization.SpaceDisk * 1024;
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

                        //walidation
                        if (model.Current.Details == null || !model.Current.Details.Any())
                        {
                            model.ErrorMessage = "Nie mozna zapisac szkolenia. Nalezy dodac element do szkolenia";
                            return false;

                        }

                        var toModified = context.Trainings.FirstOrDefault(x => x.TrainingID == model.Current.TrainingID);

                        //check training resources
                        var fileName = Path.GetFileName(toModified.TrainingResources);

                        //Assets\34c3cd6d0bbb4001a5570372f4db649f\Resources\U_05_0238035_0115RW.pdf
                        serverPath = toModified.TrainingResources.Replace("\\" + fileName, "");
                        path = Path.Combine(HttpRuntime.AppDomainAppPath, serverPath);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        fileSize = 0m;
                        if (!model.Current.TrainingResources.EndsWith(fileName))
                        {
                            File.Delete(Path.Combine(HttpRuntime.AppDomainAppPath, toModified.TrainingResources));
                            toModified.TrainingResources = AppSettings.CopyFile(path, HttpRuntime.AppDomainAppPath, model.Current.TrainingResources, out fileSize, true, "training_image");
                        }

                        fileName = String.IsNullOrEmpty(toModified.PassResources) ? string.Empty : Path.GetFileName(toModified.PassResources);

                        if ((!String.IsNullOrEmpty(model.Current.PassResources) && !model.Current.PassResources.EndsWith(fileName)) ||
                            (String.IsNullOrEmpty(fileName) && !String.IsNullOrEmpty(model.Current.PassResources)))
                        {
                            fileSize = 0m;
                            if (!String.IsNullOrEmpty(fileName))
                            {
                                File.Delete(Path.Combine(HttpRuntime.AppDomainAppPath, toModified.PassResources));
                            }
                            toModified.PassResources = AppSettings.CopyFile(path, HttpRuntime.AppDomainAppPath, model.Current.PassResources, out fileSize, true, "training_mark");
                        }
                        toModified.Name = model.Current.Name;
                        toModified.Description = model.Current.Description;
                        toModified.PassInfo = model.Current.PassInfo;
                        toModified.PassResult = model.Current.PassResult;
                        toModified.TrainingType = isInternal ? TrainingType.Internal : TrainingType.Kenpro;
                        toModified.ModifieddUserID = model.LoggedUser.Id;
                        toModified.ModifiedDate = DateTime.Now;
                        toModified.IsForAll = model.Current.IsForAll;

                        context.Entry(toModified).State = EntityState.Modified;
                        context.SaveChanges();

                        index = 0;
                        //todo add check if exists


                        if (model.Current.Details != null)
                        {
                            if (isInternal)
                            {
                                availableSpace = availableSpace - AppSettings.GetUsedSpace(context, model.CurrentOrganization.OrganizationID);
                            }

                            var modifiedDetails = context.TrainingDetails.Where(x => x.TrainingID == model.Current.TrainingID).ToList();

                            var localFile = String.Empty;

                            try
                            {

                                foreach (var item in modifiedDetails)
                                {
                                    var removed = model.Current.Details.FirstOrDefault(x => x.TrainingDetailID == item.TrainingDetailID);

                                    if (removed == null)
                                    {
                                        if (!String.IsNullOrEmpty(item.InternalResource))
                                        {
                                            //
                                            localFile = item.InternalResource.Replace("File", "C:\\Assets");
                                            File.Delete(Path.Combine(HttpRuntime.AppDomainAppPath, localFile));
                                        }
                                        if (isInternal)
                                        {
                                            availableSpace += item.FileSize;
                                        }
                                        context.Entry(item).State = EntityState.Deleted;

                                    }
                                    if (removed != null && !String.IsNullOrEmpty(item.InternalResource) && !item.Name.Equals(removed.Name))
                                    {
                                        localFile = item.InternalResource.Replace("File", "C:\\Assets");
                                        File.Delete(Path.Combine(HttpRuntime.AppDomainAppPath, localFile));
                                        if (isInternal)
                                        {
                                            availableSpace += item.FileSize;
                                        }
                                        context.Entry(item).State = EntityState.Deleted;
                                    }
                                }

                                foreach (var item in model.Current.Details)
                                {
                                    item.TrainingID = model.Current.TrainingID;
                                    item.DisplayNo = index;

                                    index++;

                                    var editable = modifiedDetails.FirstOrDefault(x => x.TrainingDetailID == item.TrainingDetailID);

                                    if (editable != null && !String.IsNullOrEmpty(item.InternalResource) && item.Name.Equals(editable.Name))
                                    {
                                        if (editable.DisplayNo != item.DisplayNo)
                                        {
                                            editable.DisplayNo = item.DisplayNo;
                                            context.Entry(editable).State = EntityState.Modified;
                                        }
                                        continue;
                                    }

                                    //move file and save size in MG
                                    if (!String.IsNullOrEmpty(item.InternalResource))
                                    {
                                        fileSize = 0m;
                                        item.InternalResource = AppSettings.CopyFile(path, HttpRuntime.AppDomainAppPath, item.InternalResource, out fileSize);
                                        item.FileSize = fileSize;
                                        if (isInternal)
                                        {
                                            availableSpace -= fileSize;

                                            if (availableSpace <= 0)
                                            {
                                                foreach (var itemToDel in model.Details.Where(x => x.FileSize != 0))
                                                {
                                                    localFile = item.InternalResource.Replace("File", "C:\\Assets");
                                                    File.Delete(Path.Combine(HttpRuntime.AppDomainAppPath, localFile));
                                                }

                                                model.ErrorMessage = "Przestrzeń dyskowa organizacji została wykorzystana i nie ma miejsca na Twoje szkolenie - skontaktuj się z administratorem.";
                                                return false;
                                            }
                                        }
                                        context.Entry(item).State = EntityState.Added;
                                    }
                                }


                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                model.ErrorMessage = "Problem z zapisaniem zmian w szczegolach szkolenia";
                                return false;
                            }
                        }

                        index = 0;


                        if (model.Current.Questions != null)
                        {
                            var modifiedQuestion = context.TrainingQuestons.Where(x => x.TrainingID == model.Current.TrainingID).ToList();
                            foreach (var modif in modifiedQuestion)
                            {
                                //modif.Answers = context.TrainingAnswers.Where(x => x.TrainingQuestionID == modif.TrainingQuestionID).ToList();
                                context.Entry(modif).State = EntityState.Deleted;
                            }

                            foreach (var item in model.Current.Questions)
                            {
                                item.TrainingID = model.Current.TrainingID;
                                item.DisplayNo = index;
                                index++;
                                context.Entry(item).State = EntityState.Added;
                            }
                            context.SaveChanges();
                        }

                        //todo assigned group
                        if (isInternal)
                        {
                            //todo change to only update

                            var currentGroups = (from t in context.TrainingInGroups
                                                 where t.TrainingID == model.Current.TrainingID
                                                 select t).ToList();

                            if (currentGroups != null)
                            {
                                foreach (var item in currentGroups)
                                {
                                    var grp = context.TrainingInGroups.FirstOrDefault(x => x.ProfileGroupID == item.ProfileGroupID && x.TrainingID == model.Current.TrainingID);
                                    if (grp != null)
                                    {
                                        context.TrainingInGroups.Remove(grp);
                                    }
                                }
                            }


                            if (!model.Current.IsForAll && model.Current.Groups != null && model.Current.Groups.Any())
                            {
                                foreach (var item in model.Current.Groups)
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
                                var trainingInOrganization = context.TrainingsInOrganizations.FirstOrDefault(x => x.OrganizationID == item.OrganizationID && x.TrainingID == model.Current.TrainingID);
                                if (trainingInOrganization != null)
                                {
                                    context.TrainingsInOrganizations.Remove(trainingInOrganization);
                                }
                            }

                            if (!model.Current.IsForAll && model.Organizations != null && model.Organizations.Any())
                            {

                                foreach (var item in model.Organizations)
                                {
                                    var trainingInOrganization = new Trainings2Organizations();
                                    trainingInOrganization.OrganizationID = item.OrganizationID;
                                    trainingInOrganization.TrainingID = model.Current.TrainingID;
                                    trainingInOrganization.IsDeleted = false;
                                    context.TrainingsInOrganizations.Add(trainingInOrganization);
                                }
                            }
                        }


                        context.SaveChanges();

                        if (isInternal)
                        {
                            LogService.InsertTrainingLogs(OperationLog.TrainingEdit, context, model.Current.TrainingID, model.LoggedUser.Id, model.CurrentOrganization.OrganizationID);
                        }

                        model.Success = "Edycja szkolenia zakonczylo sie sukcesem!";

                        break;
                    case AppEngine.Models.DataBusiness.BaseActionType.Add:

                        if (model.Details == null || !model.Details.Any())
                        {
                            model.ErrorMessage = "Nie mozna zapisac szkolenia. Nalezy dodac element do szkolenia";
                            return false;

                        }

                        model.Current.CreateDate = DateTime.Now;
                        model.Current.CreateUserID = model.LoggedUser.Id;

                        model.Current.IsDeleted = false;
                        model.Current.IsActive = true;
                        model.Current.TrainingType = isInternal ? TrainingType.Internal : TrainingType.Kenpro;

                        context.Trainings.Add(model.Current);
                        context.SaveChanges();


                        serverPath = AppSettings.ServerPath(model.Current.TrainingID);
                        path = Path.Combine(HttpRuntime.AppDomainAppPath, serverPath);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }


                        if (String.IsNullOrEmpty(model.Current.TrainingResources))
                        {
                            model.Current.TrainingResources = @"Assets\Image\main_image.png";
                        }

                        fileSize = 0m;
                        model.Current.TrainingResources = AppSettings.CopyFile(path, HttpRuntime.AppDomainAppPath, model.Current.TrainingResources, out fileSize, true, "training_image");
                        if (!String.IsNullOrEmpty(model.Current.PassResources))
                        {
                            fileSize = 0m;
                            model.Current.PassResources = AppSettings.CopyFile(path, HttpRuntime.AppDomainAppPath, model.Current.PassResources, out fileSize, true, "training_mark");
                        }

                        context.Entry<Training>(model.Current).State = EntityState.Modified;
                        context.SaveChanges();


                        index = 0;

                        if (model.Details != null && model.Details.Any())
                        {
                            if (isInternal)
                            {
                                availableSpace = availableSpace - AppSettings.GetUsedSpace(context, model.CurrentOrganization.OrganizationID);
                            }

                            foreach (var item in model.Details)
                            {
                                item.DisplayNo = index;
                                index++;

                                //move file and save size in MG
                                if (!String.IsNullOrEmpty(item.InternalResource))
                                {
                                    fileSize = 0m;
                                    item.InternalResource = AppSettings.CopyFile(path, HttpRuntime.AppDomainAppPath, item.InternalResource, out fileSize);
                                    item.FileSize = fileSize;

                                    if (isInternal)
                                    {
                                        availableSpace -= fileSize;

                                        if (availableSpace <= 0)
                                        {
                                            foreach (var itemToDel in model.Details.Where(x => x.FileSize != 0))
                                            {
                                                File.Delete(Path.Combine(HttpRuntime.AppDomainAppPath, itemToDel.InternalResource));
                                            }

                                            model.ErrorMessage = "Przestrzeń dyskowa organizacji została wykorzystana i nie ma miejsca na Twoje szkolenie - skontaktuj się z administratorem.";
                                            return false;
                                        }
                                    }
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

                        if (isInternal)
                        {
                            LogService.InsertTrainingLogs(OperationLog.TrainingCreate, context, model.Current.TrainingID, model.LoggedUser.Id, model.CurrentOrganization.OrganizationID);

                            if (model.Current.IsForAll)
                            {
                                model.Current.Groups = null;
                            }

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
                            trainingInOrganization.TrainingID = model.Current.TrainingID;
                            trainingInOrganization.IsDeleted = false;
                            context.TrainingsInOrganizations.Add(trainingInOrganization);
                            context.SaveChanges();

                        }
                        else
                        {
                            if (model.Current.IsForAll)
                            {
                                model.Organizations = new List<Organization>();
                            }

                            foreach (var item in model.Organizations)
                            {
                                var trainingInOrganization = new Trainings2Organizations();
                                trainingInOrganization.OrganizationID = item.OrganizationID;
                                trainingInOrganization.TrainingID = model.Current.TrainingID;
                                trainingInOrganization.IsDeleted = false;
                                context.TrainingsInOrganizations.Add(trainingInOrganization);
                            }

                            context.SaveChanges();
                        }

                        model.ActionType = BaseActionType.GetExtData;

                        ManageTrainings(context, model, isInternal);

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
                        model.Current.TrainingResources = @"Assets\Image\main_image.png";
                        model.Current.IsForAll = true;
                        model.Details = new List<TrainingDetail>();
                        model.Questions = new List<TrainingQuestion>();
                        model.AvailableForAll = true;

                        if (isInternal)
                        {
                            model.Groups = (from gio in context.GroupsInOrganizations
                                            join g in context.Groups on gio.ProfileGroupID equals g.ProfileGroupID
                                            where gio.OrganizationID == model.CurrentOrganization.OrganizationID && !g.IsDeleted && g.Name != "Wszyscy"
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

                        foreach (var question in model.Current.Questions)
                        {
                            question.Answers = context.TrainingAnswers.Where(x => x.TrainingQuestionID == question.TrainingQuestionID).ToList();
                        }

                        if (isInternal)
                        {
                            //todo groups
                            model.Current.Groups = (from extTig in context.TrainingInGroups
                                                    join extG in context.Groups on extTig.ProfileGroupID equals extG.ProfileGroupID
                                                    where extTig.TrainingID == model.Current.TrainingID
                                                    select extG).ToList();
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
                                var creator = context.Users.FirstOrDefault(x => x.Id == item.CreatorID);
                                item.CreatorName = creator.UserName;
                                item.CreatorLogin = creator.DisplayName;
                            }

                            item.AssignedGroup = (from tig in context.TrainingInGroups
                                                  join g in context.Groups
                                                  on tig.ProfileGroupID equals g.ProfileGroupID
                                                  where tig.TrainingID == item.Id
                                                  select g.Name).ToList();
                        }

                        model.Success = String.Empty;

                        model.InternalTrainings.AddRange(collection);

                        break;
                    case BaseActionType.GetByCreateUser:

                        if (isInternal)
                        {
                            model.Trainings = (from t in context.Trainings
                                               join to in context.TrainingsInOrganizations on t.TrainingID equals to.TrainingID
                                               where to.OrganizationID == model.CurrentOrganization.OrganizationID &&
                                               t.TrainingType == TrainingType.Internal &&
                                               t.CreateUserID == model.LoggedUser.Id
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

                    case BaseActionType.ChangeImage:

                        var toImageModified = context.Trainings.FirstOrDefault(x => x.TrainingID == model.Current.TrainingID);

                        //check training resources
                        var imageFileName = Path.GetFileName(toImageModified.TrainingResources);

                        //Assets\34c3cd6d0bbb4001a5570372f4db649f\Resources\U_05_0238035_0115RW.pdf
                        serverPath = toImageModified.TrainingResources.Replace("\\" + imageFileName, "");
                        path = Path.Combine(HttpRuntime.AppDomainAppPath, serverPath);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        fileSize = 0m;
                        if (!model.Current.TrainingResources.EndsWith(imageFileName))
                        {
                            File.Delete(Path.Combine(HttpRuntime.AppDomainAppPath, toImageModified.TrainingResources));
                            toImageModified.TrainingResources = AppSettings.CopyFile(path, HttpRuntime.AppDomainAppPath, model.Current.TrainingResources, out fileSize, true, "training_image");
                        }

                        toImageModified.ModifieddUserID = model.LoggedUser.Id;
                        toImageModified.ModifiedDate = DateTime.Now;

                        context.Entry(toImageModified).State = EntityState.Modified;
                        context.SaveChanges();

                        if (isInternal)
                        {
                            LogService.InsertTrainingLogs(OperationLog.TrainingEdit, context, model.Current.TrainingID, model.LoggedUser.Id, model.CurrentOrganization.OrganizationID);
                        }

                        model.Success = "Edycja szkolenia zakończyła się sukcesem!";

                        break;
                    default:
                        break;
                }



                return true;
            }
            catch (Exception ex)
            {
                model.ErrorMessage = "Wystapil nieokreslony problem przy probie polaczenia z baza";
                return false;
            }

        }

    }
}
