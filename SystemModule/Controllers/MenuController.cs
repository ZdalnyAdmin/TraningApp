using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemModule.Controllers
{
    public class MenuController : Controller
    {
        #region Private Fields
        private EFContext _db = new EFContext();
        #endregion

        public ActionResult Index()
        {
            return View();
        }
    }
}