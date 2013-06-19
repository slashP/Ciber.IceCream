using System;
using System.Web.Mvc;
using System.Web.Security;

namespace CiberIs.Controllers
{
    [Authorize()]
    public class RoleAdminController : Controller
    {
        //
        // GET: /RoleAdmin/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Users = "perhel")]
        public ActionResult Role()
        {
            return View();
        }

        public string AddUser(string name)
        {
            try
            {
                Roles.AddUserToRole(name, "admin");
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return string.Format("Added {0} as admin", name);
        }

        [Authorize(Users = "perhel")]
        public string AddRole(string name)
        {
            try
            {
                Roles.CreateRole(name);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return string.Format("Role {0} created", name);
        }

    }
}
