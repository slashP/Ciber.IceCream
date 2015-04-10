using System;
using System.Linq;
using System.Web.Mvc;
using CiberIs.Extensions;
using CiberIs.Models;

namespace CiberIs.Controllers
{
    [Authorize(Roles = "admin")]
    public class LossController : Controller
    {
        private readonly MongoDb _db = new MongoDb();

        public ActionResult Register()
        {
            var iceCreams = _db.GetCollection<IceCream>("IceCreams").ToList();
            return View(iceCreams);
        }

        public ActionResult RegisterForIce(string id)
        {
            var iceCream = _db.FindById<IceCream>(id, "IceCreams");
            return View(iceCream);
        }

        [HttpPost]
        public ActionResult RegisterForIce(string objectId, IceCream iceCream)
        {
            var ice = _db.FindById<IceCream>(objectId, "IceCreams");
            if(ice.Quantity < iceCream.Loss.SafeValue()) {
                throw new ArgumentException("Loss cannot be greater than the registered quantity");
            }
            ice.Loss = iceCream.Loss.SafeValue();
            ice.Quantity -= ice.Loss.SafeValue();
            _db.Save(ice, "IceCreams");
            return RedirectToAction("Index", "Admin");
        }
    }
}
