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


                if (obj.LoggedUser == null)
                {
                    obj.LoggedUser = Person.GetLoggedPerson(User);

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

                if(obj.Setting == null)
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
                        obj.Setting.AllowUserToChangeMail = setting.AllowUserToChangeMail;
                        obj.Setting.AllowUserToChangeName = setting.AllowUserToChangeName;
                        obj.Setting.DefaultEmail = setting.DefaultEmail;
                        obj.Setting.DefaultName = setting.DefaultName;
                        obj.Setting.IsDefault = false;
                        obj.Setting.IsGlobalAvailable = setting.IsGlobalAvailable;
                        obj.Setting.IsTrainingAvailableForAll = setting.IsTrainingAvailableForAll;
                        obj.Setting.MaxActiveTrainings = setting.MaxActiveTrainings;
                        obj.Setting.MaxAssignedUser = setting.MaxAssignedUser;
                        obj.Setting.SpaceDisk = setting.SpaceDisk;
                        obj.Setting.ProtectorID = obj.LoggedUser.Id;
                        db.AppSettings.Add(obj.Setting);
                        db.SaveChanges();
                    }
                    else
                    {
                        obj.Setting = setting;
                    }
                }

                switch (obj.ActionType)
                {
                    case BaseActionType.Get:
                        obj.Success = "Dane wczytane!";
                        return Request.CreateResponse(HttpStatusCode.Created, obj);
                    case BaseActionType.Edit:
                        db.Entry(obj.CurrentOrganization).State = EntityState.Modified;
                        db.SaveChanges();
                        obj.Success = "Dane zapisane!";
                        return Request.CreateResponse(HttpStatusCode.Created, obj);
                    case BaseActionType.Add:
                        db.Entry(obj.Setting).State = EntityState.Modified;
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
