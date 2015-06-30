using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    public class FileController : Controller
    {
        #region Private Fields
        private EFContext _db = new EFContext();
        #endregion

        public ActionResult Index(string guid, string fileName)
        {
            var loggedPerson = Person.GetLoggedPerson(User);
            var guidParts = guid.Split('_');
            if (guidParts.Length < 2)
            {
                return new HttpNotFoundResult();
            }

            var fileParts = fileName.Split('_');
            var name = fileName.Split('.').FirstOrDefault();

            if (loggedPerson == null)
            {
                return new HttpNotFoundResult();
            }

            if (!name.Equals("main_image"))
            {
                if (fileParts.Length < 3)
                {
                    return new HttpNotFoundResult();
                }

                var creatorId = fileParts[0];
                var creator = _db.Users.FirstOrDefault(x => x.Id == creatorId);

                int trainingId = 0;
                int.TryParse(guidParts[1], out trainingId);

                var training = _db.Trainings.FirstOrDefault(x => x.TrainingID == trainingId);

                if (creator == null || training == null)
                {
                    return new HttpNotFoundResult();
                }

                var trainingResult = _db.TrainingResults.FirstOrDefault(x => x.TrainingID == trainingId && loggedPerson.Id == x.PersonID);

                if (trainingResult == null &&
                    training.CreateUserID != loggedPerson.Id &&
                    loggedPerson.Profile != ProfileEnum.Superuser &&
                    !(loggedPerson.OrganizationID == creator.OrganizationID &&
                     (loggedPerson.Profile != ProfileEnum.Protector ||
                      loggedPerson.Profile != ProfileEnum.Manager ||
                      loggedPerson.Profile != ProfileEnum.Administrator)))
                {
                    return new HttpNotFoundResult();
                }
            }

            string filePath = Path.Combine("C:\\Assets\\" + guid + "\\" + fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return new HttpNotFoundResult();
            }

            return base.File(filePath, MimeMapping.GetMimeMapping(filePath));
        }
    }
}