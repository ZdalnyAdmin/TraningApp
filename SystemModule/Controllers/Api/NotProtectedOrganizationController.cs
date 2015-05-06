using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SystemModule.Controllers.Api
{
    public class NotProtectedOrganizationController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Organization> Get()
        {
            //get from correct profile
            return db.Organizations.Where(x => x.ProtectorID == null && x.Status == OrganizationEnum.Active);
        }
    }
}
