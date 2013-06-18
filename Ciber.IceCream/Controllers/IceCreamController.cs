using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using CiberIs.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;

namespace CiberIs.Controllers
{
    public class IceCreamController : ApiController
    {
        private readonly MongoDatabase _mongoDb = MongoHqConfig.RetrieveMongoHqDb();

        public IEnumerable<dynamic> GetIceCreams()
        {
            return _mongoDb.GetCollection<IceCream>("IceCreams").AsQueryable().Where(x => x.Quantity > 0).Select(x => new
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
            if (!includeAll)
            {
                return GetIceCreams();
            }
            return _mongoDb.GetCollection<IceCream>("IceCreams").AsQueryable().Select(x => new {
                x.Title,
                x.Price,
                Id = x.Id.ToString(),
                x.Image,
                x.Quantity
            }).ToList();
        }

        public dynamic Post(IceCream iceCream)
        {
            if (iceCream == null) throw new HttpResponseException(HttpStatusCode.BadRequest);
            try
            {
                _mongoDb.GetCollection<IceCream>("IceCreams").Insert(iceCream);
            }
            catch (MongoException e)
            {
                return new {success = false, errorMessage = e.Message};
            }
            return new { success = true, errorMessage = string.Empty, iceCreamId = iceCream.Id.ToString() };
        }

        // Fill freezer
        public dynamic Put(int quantity, string iceCreamId, int price)
        {
            if (quantity == 0 || iceCreamId == null) throw new HttpResponseException(HttpStatusCode.BadRequest);
            var iceCream = _mongoDb.GetCollection<IceCream>("IceCreams").FindOneById(new ObjectId(iceCreamId));
            if (iceCream == null) return new { success = false, errorMessage = "No ice cream with that id" };
            try
            {
                var newPrice = GetPriceBasedOnQuantity(iceCream, price, quantity);
                iceCream.Quantity += quantity;
                iceCream.Price = newPrice;
                _mongoDb.GetCollection<IceCream>("IceCreams").Save(iceCream);
            }
            catch (MongoException e)
            {
                return new { success = false, errorMessage = e.Message };
            }
            return new { success = true, errorMessage = string.Empty};
        }

        private static int GetPriceBasedOnQuantity(IceCream iceCream, int price, int quantity)
        {
            if (iceCream.Quantity == 0 || iceCream.Price == 0)
            {
                return price;
            }
            var totalQuantity = quantity + iceCream.Quantity;
            return (int)Math.Ceiling((decimal)(quantity / totalQuantity * price) + iceCream.Quantity / totalQuantity * iceCream.Price);
        }
    }
}
