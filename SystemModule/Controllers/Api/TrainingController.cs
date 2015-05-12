using AppEngine.Models.Common;
using AppEngine.Models.DataContext;
using AppEngine.Services;
using AppEngine.ViewModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SystemModule.Controllers.Api
{
    public class TrainingController : ApiController
    {
        private EFContext db = new EFContext();

        public HttpResponseMessage Post(TrainingViewModel obj)
        {

            if (obj.LoggedUser == null)
            {
                obj.LoggedUser = Person.GetLoggedPerson(User);
            }

            var result = TrainingService.ManageTrainings(db, obj, true);

            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.Created, obj);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, obj.ErrorMessage);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
