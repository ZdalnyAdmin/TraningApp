using AppEngine.Models;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using System;
using System.Linq;

namespace AppEngine.Services
{
    public class LogService
    {
        private static AppLog InitLogs(bool isSystem, OperationLog operationType, SystemLog systemType, string modifiedUserID)
        {
            var log = new AppLog();
            log.IsSystem = isSystem;
            if (isSystem)
            {
                log.SystemType = systemType;
            }
            else
            {
                log.OperationType = operationType;

            }
            var currentDate = DateTime.Now;
            log.ModifiedDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, 0);
            log.ModifiedUserID = modifiedUserID;
            return log;
        }


        /// <summary>
        /// user change logs
        /// </summary>
        /// <param name="type"></param>
        /// <param name="db"></param>
        /// <param name="user"></param>
        /// <param name="modifiedUser"></param>
        public static void InsertUserLogs(OperationLog type, EFContext db, string userID, string modifiedUserID)
        {
            try
            {
                var log = InitLogs(false, type, 0, modifiedUserID);
                log.PersonID = userID;


                var user = db.Users.FirstOrDefault(x => x.Id == modifiedUserID);

                if (user.Profile == ProfileEnum.Protector)
                {
                    var organization = db.Organizations.FirstOrDefault(x => x.ProtectorID == user.Id);
                    if (organization != null)
                    {
                        log.OrganizationID = organization.OrganizationID;
                    }
                }
                else
                {
                    log.OrganizationID = user.OrganizationID.HasValue ? user.OrganizationID.Value : 0;
                }


                bool canSave = false;
                switch (type)
                {
                    case OperationLog.UserInvitation:
                        //todo rola i mail - after send save user in person if no chnage logic
                        //log.PersonID = 

                        break;
                    case OperationLog.UserDeleteBySelf:
                    case OperationLog.UserDelete:
                    case OperationLog.UserCreate:
                    case OperationLog.UserEdit:
                        canSave = true;
                        break;
                }

                if (!canSave)
                {
                    return;
                }

                db.Logs.Add(log);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// training change logs
        /// </summary>
        /// <param name="type"></param>
        /// <param name="db"></param>
        /// <param name="training"></param>
        /// <param name="modifiedUser"></param>
        public static void InsertTrainingLogs(OperationLog type, EFContext db, int trainingID, string modifiedUserID)
        {
            var log = InitLogs(false, type, 0, modifiedUserID);
            log.TrainingID = trainingID;

            var user = db.Users.FirstOrDefault(x => x.Id == modifiedUserID);

            if (user.Profile == ProfileEnum.Protector)
            {
                var organization = db.Organizations.FirstOrDefault(x => x.ProtectorID == user.Id);
                if (organization != null)
                {
                    log.OrganizationID = organization.OrganizationID;
                }
            }
            else
            {
                log.OrganizationID = user.OrganizationID.HasValue ? user.OrganizationID.Value : 0;
            }

            bool canSave = false;
            switch (type)
            {
                case OperationLog.TrainingCreate:
                case OperationLog.TrainingEdit:
                    canSave = true;
                    break;
            }

            if (!canSave)
            {
                return;
            }

            db.Logs.Add(log);
            db.SaveChanges();
        }

        public static void OrganizationLogs(SystemLog type, EFContext db, string organizationName, string modifiedUserID)
        {
            try
            {
                var log = InitLogs(true, 0, type, modifiedUserID);
                log.OrganizationName = organizationName;

                bool canSave = false;
                switch (type)
                {
                    case SystemLog.OrganizationCreate:
                    case SystemLog.OrganizationDelete:
                    case SystemLog.OrganizationRequestToRemove:
                        canSave = true;
                        break;
                }

                if (!canSave)
                {
                    return;
                }

                db.Logs.Add(log);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public static void ProtectorLogs(SystemLog type, EFContext db, string organizationName, string modifiedUserID)
        {
            try
            {
                var log = InitLogs(true, 0, type, modifiedUserID);
                log.OrganizationName = organizationName;

                bool canSave = false;
                switch (type)
                {
                    case SystemLog.ProtectorCreate:
                    case SystemLog.ProtectorInvitation:
                        canSave = true;
                        break;
                }

                if (!canSave)
                {
                    return;
                }

                db.Logs.Add(log);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public static void AdministrationLogs(SystemLog type, EFContext db, string modifiedUserID)
        {
            try
            {
                var log = InitLogs(true, 0, type, modifiedUserID);

                bool canSave = false;
                switch (type)
                {
                    case SystemLog.LogIn:
                    case SystemLog.LogOut:
                        canSave = true;
                        break;
                }

                if (!canSave)
                {
                    return;
                }

                db.Logs.Add(log);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }

        }

    }
}
