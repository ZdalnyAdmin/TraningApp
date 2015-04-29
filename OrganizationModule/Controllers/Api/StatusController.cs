using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppEngine.Helpers;

namespace OrganizationModule.Controllers
{
    public class StatusController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var statuses = new List<string>();

            foreach(StatusEnum status in Enum.GetValues(typeof(StatusEnum)))
                statuses.Add(status.GetDescription());

            return statuses;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
