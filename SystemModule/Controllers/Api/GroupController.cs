using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SystemModule.Controllers.Api
{
    public class GroupController : ApiController
    {
        private EFContext db = new EFContext();

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<ProfileGroup> Get()
        {
            var groups = db.Groups.Where(x => !x.IsDeleted).ToList();

            foreach (var item in groups)
            {
                var people = (from pg in db.PeopleInGroups
                              join p in db.Users on pg.PersonID equals p.Id
                              where pg.ProfileGroupID == item.ProfileGroupID
                              select p).ToList();
                item.AssignedPeople = people;
            }
            return groups;
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
