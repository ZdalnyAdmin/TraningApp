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
            var fileParts = fileName.Split('_');
            if (fileParts.Length < 3)
            {
                return new HttpNotFoundResult();
            }
            var creatorId = fileParts[0];
            var creator = _db.Users.FirstOrDefault(x => x.Id == creatorId);
            
            if(creator == null || loggedPerson == null || (creator.Profile != ProfileEnum.Superuser && creator.OrganizationID != loggedPerson.OrganizationID))
            {
                return new HttpNotFoundResult();
            }

            string filePath = Path.Combine("C:\\Assets\\" + guid + "\\Resources\\" + fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return new HttpNotFoundResult();
            }

            return base.File(filePath, MimeMapping.GetMimeMapping(filePath));
        }
    }
}