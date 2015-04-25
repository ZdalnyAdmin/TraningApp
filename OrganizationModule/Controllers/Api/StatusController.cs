using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrganizationModule.Controllers
{
    public class StatusController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Status> Get()
        {
            var status = db.Status.AsEnumerable().ToList();
            return status;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
