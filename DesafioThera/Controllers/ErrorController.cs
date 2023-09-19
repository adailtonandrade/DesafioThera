using System.Web.Mvc;

namespace DesafioThera.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult Index(int? code)
        {
            return View("Error");
        }

        public ActionResult AccessDenied()
        {
            //Response.TrySkipIisCustomErrors = true;
            //Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return View();
        }

        public ActionResult NotFound()
        {
            //Response.TrySkipIisCustomErrors = true;
            //Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View();
        }

        public ActionResult ErrorCustom()
        {
            return View();
        }

        public ActionResult ErrorInternalServer()
        {
            //Response.TrySkipIisCustomErrors = true;
            //Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}