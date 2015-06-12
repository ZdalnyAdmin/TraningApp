using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DataObject;
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
    public class SettingsController : ApiController
    {
        private EFContext db = new EFContext();

        // POST api/<controller>
        public HttpResponseMessage Post(OrganizationViewModel obj)
        {
            try
            {
                //check if object exist 
                if (obj == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }


                obj.LoggedUser = Person.GetLoggedPerson(User);
                if (obj.LoggedUser.Status == StatusEnum.Deleted)
                {
                    obj.ErrorMessage = "Uprawnienia uzytkownika wygasly!";
                    return Request.CreateResponse(HttpStatusCode.Created, obj);
                }

                if (obj.LoggedUser.Profile != ProfileEnum.Protector)
                {
                    obj.ErrorMessage = "Zalogowany użytkowanik nie jest Opiekunem. Brak uprawnien do modyfikacji.";
                    return Request.CreateResponse(HttpStatusCode.Created, obj);
                }

                if (!obj.LoggedUser.OrganizationID.HasValue || obj.LoggedUser.OrganizationID == 0)
                {
                    obj.ErrorMessage = "Zalogowany użytkowanik nie posiada przypisanej organizacji.";
                    return Request.CreateResponse(HttpStatusCode.Created, obj);
                }

                if (obj.CurrentOrganization == null)
                {
                    obj.CurrentOrganization = db.Organizations.FirstOrDefault(x => x.OrganizationID == obj.LoggedUser.OrganizationID);
                }

                if (obj.Setting == null)
                {
                    //get setting for organization
                    var setting = db.AppSettings.FirstOrDefault(x => x.ProtectorID == obj.LoggedUser.Id);
                    if (setting == null)
                    {
                        setting = db.AppSettings.FirstOrDefault(x => x.IsDefault);
                    }

                    if (setting == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }

                    if (setting.IsDefault)
                    {
                        obj.Setting = new AppSetting();
                        obj.Setting.AllowUserToChangeMail = obj.CurrentOrganization.CanUserChangeMail;
                        obj.Setting.AllowUserToChangeName = obj.CurrentOrganization.CanUserChangeName;
                        obj.Setting.DefaultEmail = setting.DefaultEmail;
                        obj.Setting.DefaultName = setting.DefaultName;
                        obj.Setting.IsDefault = false;
                        obj.Setting.IsGlobalAvailable = obj.CurrentOrganization.IsGlobalAvailable;
                        obj.Setting.IsTrainingAvailableForAll = obj.CurrentOrganization.IsTrainingAvailableForAll;
                        obj.Setting.MaxActiveTrainings = setting.MaxActiveTrainings;
                        obj.Setting.MaxAssignedUser = obj.CurrentOrganization.MaxAssignedUser;
                        obj.Setting.SpaceDisk = System.Convert.ToInt32(obj.CurrentOrganization.SpaceDisk);
                        obj.Setting.ProtectorID = obj.CurrentOrganization.ProtectorID;
                        db.AppSettings.Add(obj.Setting);
                        db.SaveChanges();
                    }
                    else
                    {
                        setting.AllowUserToChangeMail = obj.CurrentOrganization.CanUserChangeMail;
                        setting.AllowUserToChangeName = obj.CurrentOrganization.CanUserChangeName;
                        setting.IsGlobalAvailable = obj.CurrentOrganization.IsGlobalAvailable;
                        setting.IsTrainingAvailableForAll = obj.CurrentOrganization.IsTrainingAvailableForAll;
                        setting.MaxAssignedUser = obj.CurrentOrganization.MaxAssignedUser;
                        setting.SpaceDisk = System.Convert.ToInt32(obj.CurrentOrganization.SpaceDisk);
                        setting.ProtectorID = obj.CurrentOrganization.ProtectorID;
                        obj.Setting = setting;
                    }
                }

                switch (obj.ActionType)
                {
                    case BaseActionType.Get:
                        obj.Success = String.Empty;
                        return Request.CreateResponse(HttpStatusCode.Created, obj);
                    case BaseActionType.Edit:

                        db.Entry(obj.CurrentOrganization).State = EntityState.Modified;

                        //add changes in settings
                        obj.Setting.AllowUserToChangeMail = obj.CurrentOrganization.CanUserChangeMail;
                        obj.Setting.AllowUserToChangeName = obj.CurrentOrganization.CanUserChangeName;

                        db.Entry(obj.Setting).State = EntityState.Modified;


                        db.SaveChanges();
                        obj.Success = "Dane zapisane!";
                        return Request.CreateResponse(HttpStatusCode.Created, obj);
                    case BaseActionType.Add:


                        db.Entry(obj.Setting).State = EntityState.Modified;

                        //assigned new values into organization 

                        obj.CurrentOrganization.CanUserChangeMail = obj.Setting.AllowUserToChangeMail;
                        obj.CurrentOrganization.CanUserChangeName = obj.Setting.AllowUserToChangeName;
                        obj.CurrentOrganization.IsGlobalAvailable = obj.Setting.IsGlobalAvailable;
                        obj.CurrentOrganization.IsTrainingAvailableForAll = obj.Setting.IsTrainingAvailableForAll;
                        obj.CurrentOrganization.MaxAssignedUser = obj.Setting.MaxAssignedUser;
                        obj.CurrentOrganization.SpaceDisk = obj.Setting.SpaceDisk;

                        db.Entry(obj.CurrentOrganization).State = EntityState.Modified;

                        db.SaveChanges();
                        obj.Success = "Dane zapisane!";
                        return Request.CreateResponse(HttpStatusCode.Created, obj);
                }

            }
            catch (Exception ex)
            {
                obj.ErrorMessage = "Nieoczekiwany blad " + ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            return Request.CreateResponse(HttpStatusCode.Created, obj);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
