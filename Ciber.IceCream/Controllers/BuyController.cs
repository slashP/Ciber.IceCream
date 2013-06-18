using System;
using System.Net;
using System.Web.Http;
using CiberIs.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CiberIs.Controllers
{
    public class BuyController : ApiController
    {
        private readonly MongoDatabase _mongoDb = MongoHqConfig.RetrieveMongoHqDb();

        public dynamic Post(string iceCreamId, int buyer)
        {
            var ice = _mongoDb.GetCollection<IceCream>("IceCreams").FindOneById(new ObjectId(iceCreamId));
            if (ice == null) throw new HttpResponseException(HttpStatusCode.ExpectationFailed);
            try
            {
                ice.Quantity--;
                _mongoDb.GetCollection<IceCream>("IceCreams").Save(ice);
                _mongoDb.GetCollection<Purchase>("Purchases").Insert(new Purchase() { Price = ice.Price, Buyer = buyer, Time = DateTime.UtcNow });
            }
            catch (MongoException e)
            {
                return new { success = false, errorMessage = e.Message };
            }
            return new { success = true, errorMessage = string.Empty };
        }
    }
}
