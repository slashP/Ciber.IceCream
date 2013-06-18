using System;
using System.Web.Http;
using CiberIs.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CiberIs.Controllers
{
    public class PurchaseIceController : ApiController
    {
        private readonly MongoDatabase _mongoDb = MongoHqConfig.RetrieveMongoHqDb();

        public void Put(string id, string purchasedBy)
        {
            var ice = _mongoDb.GetCollection<Ice>("Ices").FindOneById(new ObjectId(id));
            ice.PurchasedBy = purchasedBy;
            ice.PurchasedTime = DateTime.UtcNow;
            _mongoDb.GetCollection<Ice>("Ices").Save(ice);
        }
    }
}
