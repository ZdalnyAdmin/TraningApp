using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace OrganizationModule.Controllers.Api
{
    public class SimpleTrainingController : ApiController
    {

        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Training> Get()
        {
            //get from correct profile
            var result = db.Trainings.Where(x => x.IsActive && !x.IsDeleted && x.TrainingType == TrainingType.Internal).ToList();

            foreach (var item in result)
            {
                var user = db.Users.FirstOrDefault(x => x.Id == item.CreateUserID);
                if (user == null)
                {
                    continue;
                }
                item.SetCreateUserName(user.DisplayName);
            }

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
