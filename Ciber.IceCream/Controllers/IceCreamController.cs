using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using CiberIs.Models;
using MongoDB.Driver;
using System.Linq;

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
            return _mongoDb.GetCollection<IceCream>("IceCreams").Where(x => x.Quantity > 0).Select(x => new
            {
                x.Title,
                x.Price,
                Id = x.Id.ToString(),
                x.Image,
                x.Quantity
            }).ToList();
        }

        public IEnumerable<dynamic> GetIceCreams(bool includeAll)
        {
            return includeAll
                       ? _mongoDb.GetCollection<IceCream>("IceCreams").AsQueryable().Select(x => new
                           {
                               x.Title,
                               x.Price,
                               Id = x.Id.ToString(),
                               x.Image,
                               x.Quantity
                           }).ToList()
                       : GetIceCreams();
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
                var newPrice = GetPriceBasedOnQuantity(iceCream, price, quantity);
                iceCream.Quantity += quantity;
                iceCream.Price = newPrice;
                _mongoDb.Save(iceCream, "IceCreams");
            }
            catch (MongoException e)
            {
                return new { success = false, errorMessage = e.Message };
            }
            return new { success = true, errorMessage = string.Empty, quantity = iceCream.Quantity, price = iceCream.Price};
        }

        private static int GetPriceBasedOnQuantity(IceCream iceCream, int price, int quantity)
        {
            if (iceCream.Quantity == 0 || iceCream.Price == 0)
            {
                return price;
            }
            var totalQuantity = quantity + iceCream.Quantity;
            var fraction1 = (decimal) (quantity)*price;
            var fraction2 = (decimal) (iceCream.Quantity)*iceCream.Price;
            var fraction3 = (fraction1 + fraction2)/totalQuantity;
            return (int)Math.Round(fraction3, 0);
        }
    }
}
