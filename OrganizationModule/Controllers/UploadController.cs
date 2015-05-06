using AppEngine.Models.DataBusiness;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganizationModule.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        public ActionResult UploadImage()
        {
            string url = string.Empty;

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Temp/Images/"), fileName);
                    url = "/Temp/Images/" + fileName;
                    file.SaveAs(path);
                }
            }

            return Json(new Result() { Succeeded = true, Message = url });
        }

        [HttpPost]
        public ActionResult UploadMovie()
        {
            string url = string.Empty;

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Temp/Movies/"), fileName);
                    url = "/Temp/Movies/" + fileName;
                    file.SaveAs(path);
                }
            }

            return Json(new Result() { Succeeded = true, Message = url });
        }
    }
}