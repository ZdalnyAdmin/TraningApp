using AppEngine.Helpers;
using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
using AppEngine.Models.DTO;
using AppEngine.Services;
using AppEngine.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;

namespace SystemModule.Controllers.Api
{
    public class OrganizationsController : ApiController
    {
        private EFContext db = new EFContext();

        [HttpPost]
        public HttpResponseMessage Post(OrganizationViewModel obj)
        {
            try
            {
                obj.ErrorMessage = String.Empty;
                if (obj.LoggedUser == null)
                {
                    obj.LoggedUser = Person.GetLoggedPerson(User);
                }
                switch (obj.ActionType)
                {
                    case BaseActionType.Get:

                        obj.Organizations = db.Organizations.OrderByDescending(x => x.CreateDate).ToList();

                        if(obj.Organizations != null)
                        {
                            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                            currentDate = currentDate.AddHours(-72);
                            foreach (var org in obj.Organizations)
                            {
                                if (String.IsNullOrEmpty(org.NewName))
                                {
                                    org.NewName = org.Name;
                                }

                                if (!org.ChangeNameDate.HasValue || org.ChangeNameDate.Value <= currentDate)
                                {
                                    org.NewName = org.Name;
                                }
                            }
                        }

                        obj.Success = String.Empty;

                        break;
                    case BaseActionType.Delete:

                        obj.Current.IsDeleted = false;
                        obj.Current.DeletedUserID = obj.LoggedUser.Id;
                        obj.Current.Status = OrganizationEnum.Hidden;
                        db.Entry(obj.Current).State = EntityState.Modified;
                        db.SaveChanges();

                        var deleted = obj.Organizations.FirstOrDefault(x => x.OrganizationID == obj.Current.OrganizationID);
                        deleted.Status = OrganizationEnum.Hidden;

                        obj.Success = "Organizacja usunieta!";

                        break;
                    case BaseActionType.Edit:

                        db.Entry(obj.Current).State = EntityState.Modified;
                        db.SaveChanges();

                        obj.Success = "Dane organizacji zostaly zmienione!";

                        break;
                    case BaseActionType.Add:

                        obj.Current.CreateUserID = obj.LoggedUser.Id;
                        obj.Current.CreateDate = DateTime.Now;
                        obj.Current.IsDeleted = false;
                        obj.Current.Status = OrganizationEnum.Active;
                        obj.Current.UpdateSecurityStamp();

                        if (ModelState.IsValid)
                        {
                            db.Organizations.Add(obj.Current);
                            db.SaveChanges();

                            if (obj.Current.IsTrainingAvailableForAll)
                            {
                                var group = db.Groups.FirstOrDefault(x => x.Name == "Wszyscy");
                                if (group == null)
                                {
                                    obj.ErrorMessage = "Brak grupy Wszyscy. Prosze o kontakt z administratorem!";
                                    return Request.CreateResponse(HttpStatusCode.Created, obj); ;
                                }

                                var pgo = new ProfileGroup2Organization();
                                pgo.ProfileGroupID = group.ProfileGroupID;
                                pgo.OrganizationID = obj.Current.OrganizationID;
                                db.GroupsInOrganizations.Add(pgo);
                                db.SaveChanges();
                            }
                        }

                        MailMessage mail = new MailMessage(new MailAddress(Helpers.GetMailFrom(MailAccount.EVENT), "(do not reply)"),
                        new MailAddress("admin@kenis.pl"))
                        {
                            Subject = "Utworzenie organizacji",
                            Body = string.Format("W dniu {0} została utworzona nowa organizacja {1} przez {2}", DateTime.Now.ToString("dd/MM/yyyy"), obj.Current.Name, obj.LoggedUser.DisplayName),
                            IsBodyHtml = true
                        };

                        mail.BodyEncoding = UTF8Encoding.UTF8;
                        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                        Mail.Send(mail, MailAccount.EVENT);

                        LogService.OrganizationLogs(SystemLog.OrganizationCreate, db, obj.Current.Name, obj.LoggedUser.Id);

                        obj.ActionType = BaseActionType.GetExtData;
                        Post(obj);

                        obj.Success = "Organizacja zostala zapisana!";

                        break;
                    case BaseActionType.GetSimple:


                        obj.Organizations = (from t in db.Organizations
                                             orderby t.CreateDate
                                             select t).ToList();

                        if (obj.Organizations != null)
                        {
                            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                            currentDate = currentDate.AddHours(-72);
                            foreach (var org in obj.Organizations)
                            {
                                if (String.IsNullOrEmpty(org.NewName))
                                {
                                    org.NewName = org.Name;
                                }

                                if (!org.ChangeNameDate.HasValue || org.ChangeNameDate.Value <= currentDate)
                                {
                                    org.NewName = org.Name;
                                }
                            }
                        }

                        obj.Success = String.Empty;

                        break;
                    case BaseActionType.GetExtData:
                        //get settingds
                        obj.Setting = db.AppSettings.FirstOrDefault(x => x.IsDefault);
                        if (obj.Setting == null)
                        {
                            obj.Setting = new AppSetting();
                            obj.Setting.AllowUserToChangeName = true;
                            obj.Setting.AllowUserToChangeMail = true;
                            obj.Setting.SpaceDisk = 50;
                            obj.Setting.MaxAssignedUser = 10;
                            obj.Setting.IsGlobalAvailable = true;
                            obj.Setting.IsTrainingAvailableForAll = true;
                            obj.Setting.MaxActiveTrainings = 5;
                            obj.Setting.DefaultEmail = string.Empty;
                            obj.Setting.DefaultName = string.Empty;
                            obj.Setting.IsDefault = true;
                            db.AppSettings.Add(obj.Setting);
                            db.SaveChanges();
                        }
                        obj.Current = new Organization();
                        obj.Current.SpaceDisk = obj.Setting.SpaceDisk;
                        obj.Current.MaxAssignedUser = obj.Setting.MaxAssignedUser;
                        obj.Current.IsGlobalAvailable = obj.Setting.IsGlobalAvailable;
                        obj.Current.IsTrainingAvailableForAll = obj.Setting.IsTrainingAvailableForAll;
                        obj.Current.CanUserChangeMail = obj.Setting.AllowUserToChangeMail;
                        obj.Current.CanUserChangeName = obj.Setting.AllowUserToChangeName;

                        break;
                    case BaseActionType.ById:
                        obj.Detail = new OrganizationDto();

                        if (obj.OrganizationID == 0)
                        {
                            obj.ErrorMessage = "Problem z pobraniem szczegowych danych organizacji";
                            return Request.CreateResponse(HttpStatusCode.Created, obj); ;
                        }

                        var current = obj.Organizations.FirstOrDefault(x=>x.OrganizationID == obj.OrganizationID);

                        if(String.IsNullOrEmpty(current.NewName))
                        {
                            current.NewName = current.Name;
                        }

                        var protector = db.Users.FirstOrDefault(x=>x.Id == current.ProtectorID);

                        var assignedUsers = (from gio in db.GroupsInOrganizations
                                             join pig in db.PeopleInGroups on gio.ProfileGroupID equals pig.ProfileGroupID
                                             join u in db.Users on pig.PersonID equals u.Id
                                             where gio.OrganizationID == obj.OrganizationID
                                             select u).ToList();

                        obj.Detail.AssignedUser = assignedUsers.Count(x => x.Status == StatusEnum.Active);
                        obj.Detail.BlockedUser = assignedUsers.Count(x => x.Status == StatusEnum.Blocked);
                        obj.Detail.DeleteUser = assignedUsers.Count(x => x.Status == StatusEnum.Deleted);
                        obj.Detail.InvationUser = assignedUsers.Count(x => x.Status == StatusEnum.Invited);

                        obj.Detail.Email = protector != null ? protector.Email : String.Empty;

                        

                        var trainings = (from tio in db.TrainingsInOrganizations
                                             join t in db.Trainings on tio.TrainingID equals t.TrainingID
                                             join td in db.TrainingDetails on t.TrainingID equals td.TrainingID
                                             where tio.OrganizationID == obj.OrganizationID
                                             select td).ToList();

                        obj.Detail.UsedSpaceDisk = trainings.Sum(x=>x.FileSize);
                        //todo details

                        obj.Success = String.Empty;

                        break;
                    case BaseActionType.GetSpecial:
                        obj.Protector = new Person();
                        obj.Protector.Profile = ProfileEnum.Protector;


                        var setting = db.AppSettings.FirstOrDefault(x => x.IsDefault);

                        if(setting  != null)
                        {
                            obj.Protector.UserName = setting.DefaultName;
                            obj.Protector.Email = setting.DefaultEmail;
                        }

                        obj.NotAssigned = (from t in db.Organizations
                                           where t.ProtectorID == null && t.Status == OrganizationEnum.Active
                                           select new OrganizationDto
                                           {
                                               Id = t.OrganizationID,
                                               Name = t.Name
                                           }).ToList();

                        obj.Success = String.Empty;
                        break;
                    default:
                        break;
                }
                return Request.CreateResponse(HttpStatusCode.Created, obj);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
