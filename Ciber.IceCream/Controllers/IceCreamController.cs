using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using CiberIs.Models;
using MongoDB.Driver;
using System.Linq;
using CiberIs.Extensions;

namespace CiberIs.Controllers
{
    public class IceCreamController : ApiController
    {
        private readonly IMongoDb _mongoDb;

        public IceCreamController(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }

        public IEnumerable<dynamic> GetIceCreams()
        {
            return User.IsInRole("admin") ? GetIceCreams(true) : GetIceCreams(false);
        }

        public IEnumerable<dynamic> GetIceCreams(bool includeAll)
        {
            return
                _mongoDb.GetCollection<IceCream>("IceCreams")
                    .Where(x => x.Quantity > 0 || includeAll)
                    .Select(x => new { x.Title, Price = x.Price.ToInt(), Id = x.Id.ToString(), x.Image, x.Quantity })
                    .ToList();
        }

        [Authorize(Roles = "admin")]
        public dynamic Post(IceCream iceCream)
        {
            if (iceCream == null) throw new HttpResponseException(HttpStatusCode.BadRequest);
            try
            {
                _mongoDb.Insert(iceCream, "IceCreams");
            }
            catch (MongoException e)
            {
                return new {success = false, errorMessage = e.Message};
            }
            return new { success = true, errorMessage = string.Empty, iceCreamId = iceCream.Id.ToString() };
        }

        // Fill freezer
        [Authorize(Roles = "admin")]
        public dynamic Put(int quantity, string iceCreamId, int price)
        {
            if (quantity == 0 || iceCreamId == null) throw new HttpResponseException(HttpStatusCode.BadRequest);
            var iceCream = _mongoDb.FindById<IceCream>(iceCreamId, "IceCreams");
            if (iceCream == null) return new { success = false, errorMessage = "No ice cream with that id" };
            try
            {
                var newPrice = GetPriceBasedOnQuantityAndLoss(iceCream, price, quantity);
                iceCream.Quantity += quantity;
                iceCream.Price = newPrice;
                iceCream.Loss = 0;
                _mongoDb.Save(iceCream, "IceCreams");
            }
            catch (MongoException e)
            {
                return new { success = false, errorMessage = e.Message };
            }
            return new { success = true, errorMessage = string.Empty, quantity = iceCream.Quantity, price = iceCream.Price.ToInt()};
        }

        private static double GetPriceBasedOnQuantityAndLoss(IceCream iceCream, int price, int quantity)
        {
            var totalQuantity = quantity + iceCream.Quantity;
            var fraction1 = (double) (quantity)*price;
            var fraction2 = (iceCream.Quantity)*iceCream.Price;
            var fraction3 = (fraction1 + fraction2 + iceCream.Loss.SafeValue()*iceCream.Price)/totalQuantity;
            return fraction3;
        }
    }
}
