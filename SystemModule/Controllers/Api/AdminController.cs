using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SystemModule.Controllers.Api
{
    public class AdminController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return db.Users.Where(x => x.Profile == ProfileEnum.Administrator && !x.IsDeleted).OrderByDescending(p => p.RegistrationDate);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
