using System.Web.Mvc;

namespace CiberIs.Controllers
{
    public class FillFreezerController : Controller
    {
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
