using AppEngine.Models;
using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEngine.Services
{
    public class LogService
    {
        /// <summary>
        /// user change logs
        /// </summary>
        /// <param name="type"></param>
        /// <param name="db"></param>
        /// <param name="user"></param>
        /// <param name="modifiedUser"></param>
        public static void InsertUserLogs(OperationLog type, EFContext db, string userID, string modifiedUserID)
        {
            var log = new AppLog();
            log.OperationType = type;
            var currentDate = DateTime.Now;
            log.ModifiedDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, 0);
            log.ModifiedUserID = modifiedUserID;
            log.PersonID = userID;

            bool canSave = false;
            switch (type)
            {
                case OperationLog.Zaproszenie:
                    //todo rola i mail - after send save user in person if no chnage logic
                    //log.PersonID = 

                    break;
                case OperationLog.Samousuniecie:
                case OperationLog.Usuniecie:
                case OperationLog.Uzytkownik:
                case OperationLog.Edycja:
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

        /// <summary>
        /// training change logs
        /// </summary>
        /// <param name="type"></param>
        /// <param name="db"></param>
        /// <param name="training"></param>
        /// <param name="modifiedUser"></param>
        public static void InsertTrainingLogs(OperationLog type, EFContext db, int trainingID, string modifiedUserID)
        {
            var log = new AppLog();
            log.OperationType = type;
            var currentDate = DateTime.Now;
            log.ModifiedDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute, 0);
            log.ModifiedUserID = modifiedUserID;
            log.TrainingID = trainingID;

            bool canSave = false;
            switch (type)
            {
                case OperationLog.KursEdycja:
                case OperationLog.KursNowy:
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

    }
}
