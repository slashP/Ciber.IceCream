using System.Web.Mvc;
using EmptyMvc4.Filters;

namespace CiberIs.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
