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
                    .ToList()
                    .Select(x => new { x.Title, Price = x.Price.ToInt(), Id = x.Id.ToString(), x.Image, x.Quantity })
                    .Where(x => x.Quantity > 0 || includeAll);
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
                iceCream.Quantity += quantity;
                iceCream.Price = price;
                _mongoDb.Save(iceCream, "IceCreams");
            }
            catch (MongoException e)
            {
                return new { success = false, errorMessage = e.Message };
            }
            return new { success = true, errorMessage = string.Empty, quantity = iceCream.Quantity, price = iceCream.Price.ToInt()};
        }
    }
}
