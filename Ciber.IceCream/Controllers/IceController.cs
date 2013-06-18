using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CiberIs.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Wrappers;

namespace CiberIs.Controllers
{
    public class IceController : ApiController
    {
        private readonly MongoDatabase _mongoDb = MongoHqConfig.RetrieveMongoHqDb();

        public IEnumerable<dynamic> Get()
        {
            return _mongoDb.GetCollection<Ice>("Ices").AsQueryable().Where(x => x.PurchasedTime == null).Select(x => new {
                x.Price, x.ImageUrl, Id = x.Id.ToString(), x.Name
            }).ToList();
        }

        public dynamic Get(string id)
        {
            var ice = _mongoDb.GetCollection<Ice>("Ices").FindOneById(new ObjectId(id));
            return new { ice.Name, ice.PurchasedTime, ice.PurchasedBy, ice.Price };
        }

        public dynamic Post(Ice ice)
        {
            ice.PurchasedTime = null;
            ice.PurchasedBy = string.Empty;
            _mongoDb.GetCollection<Ice>("Ices").Insert(ice);
            return new { ice.Name, Id = ice.Id.ToString(), ice.Price };
        }

        public void Post(Ice ice, int numberOfItems)
        {
            if (numberOfItems == 0) throw new HttpResponseException(HttpStatusCode.BadRequest);
            ice.PurchasedTime = null;
            ice.PurchasedBy = string.Empty;
            var ices = Enumerable.Range(0, numberOfItems).Select(x => new Ice{ImageUrl = ice.ImageUrl, Name = ice.Name, Price = ice.Price, PurchasedBy = ice.PurchasedBy, PurchasedTime = ice.PurchasedTime});
            _mongoDb.GetCollection<Ice>("Ices").InsertBatch(ices);
        }

        public void Put(string id, Ice ice)
        {
            var oldIce = _mongoDb.GetCollection<Ice>("Ices").FindOneById(new ObjectId(id));
            ice.Id = oldIce.Id;
            _mongoDb.GetCollection<Ice>("Ices").Save(ice);
        }
    }
}
