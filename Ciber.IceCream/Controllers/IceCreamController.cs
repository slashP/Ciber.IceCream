using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CiberIs.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CiberIs.Controllers
{
    public class IceCreamController : ApiController
    {
        private readonly MongoDatabase _mongoDb = MongoHqConfig.RetrieveMongoHqDb();

        /*public IEnumerable<dynamic> GetPurchasedIceCreams()
        {
            return _mongoDb.GetCollection<Purchase>("Purchases").FindAll().Select(x => new {
                x.Name, x.Price, x.Time, Id = x.Id.ToString()
            }).ToList();
        }*/

        public dynamic GetIceCream(string id)
        {
            var purchase = _mongoDb.GetCollection<Purchase>("Purchases").FindOneById(new ObjectId(id));
            return new {purchase.Name, purchase.Time, purchase.Price};
        }

        public IEnumerable<dynamic> GetIceCreams(DateTime from, DateTime to)
        {
            return _mongoDb.GetCollection<Purchase>("Purchases").AsQueryable().Where(x => x.Time > from && x.Time < to).Select(x => new {
                x.Name, x.Price, x.Time, Id = x.Id.ToString()
                }).ToList();
        }

        public IEnumerable<dynamic> GetIceCreams(DateTime from, DateTime to, string name)
        {
            return _mongoDb.GetCollection<Purchase>("Purchases").AsQueryable().Where(x => x.Name.ToLower() == name.ToLower() && x.Time > from && x.Time < to).Select(x => new
            {
                x.Name,
                x.Price,
                x.Time,
                Id = x.Id.ToString()
            }).ToList();
        }

        [HttpGet]
        public IEnumerable<dynamic> GetIceCreams()
        {
            return new[]
                {
                    new
                        {
                            Title = "Krone-is",
                            Price = 8,
                            ImageURL = "/content/images/is_1.jpg"
                        },
                    new
                        {
                            Title = "Snickers is",
                            Price = 9,
                            ImageURL = "/content/images/is_2.jpg"
                        }
                };
        }

        public dynamic PostIceCream(Purchase purchase)
        {
            purchase.Time = DateTime.UtcNow;
            _mongoDb.GetCollection<Purchase>("Purchases").Insert(purchase);
            return new{purchase.Time, Id = purchase.Id.ToString(), purchase.Price, purchase.Name};
        }
    }
}