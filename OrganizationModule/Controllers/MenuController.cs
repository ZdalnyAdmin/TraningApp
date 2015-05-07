using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    public class MenuController : Controller
    {
        #region Private Fields
        private EFContext _db = new EFContext();
        #endregion

        // GET: Menu
        [ChildActionOnly]
        public ActionResult KeeperMenu()
        {
            var loggedPerson = Person.GetLoggedPerson(User);
            var trainings = new List<Training>();

            trainings = _db.Trainings
                            .Join(_db.TrainingsInOrganizations
                                     .Where(y => y.OrganizationID == loggedPerson.OrganizationID),
                                  x => x.TrainingID,
                                  y => y.TrainingID,
                                  (x, y) => x)
                            .ToList();

            ViewBag.LoggedUser = loggedPerson;
            ViewBag.Trainings = trainings;

            return View();
        }

        public ActionResult Index()
        {
            var loggedPerson = Person.GetLoggedPerson(User);
            var trainings = new List<Training>();

            ViewBag.LoggedUser = loggedPerson;
            ViewBag.Trainings = trainings;
            return View();
        }
    }
}