using System;
using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http;
using CiberIs.Models;
using MongoDB.Driver;
using CiberIs.Extensions;

namespace CiberIs.Controllers
{
    using System.Linq;

    public class BuyController : ApiController
    {
        private readonly IMongoDb _mongoDb;

        public BuyController(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }

        public dynamic Post(FormDataCollection data)
        {
            var iceCreamId = data.Get("iceCreamId");
            var ice = _mongoDb.FindById<IceCream>(iceCreamId, "IceCreams");
            if (ice == null) throw new HttpResponseException(HttpStatusCode.ExpectationFailed);
            try
            {
                ice.Quantity--;
                if(ice.Quantity < 0) throw new HttpResponseException(HttpStatusCode.Conflict);
                _mongoDb.Save(ice, "IceCreams");
                int buyerId;
                if (!int.TryParse(data.Get("buyer"), out buyerId))
                {
                    var user =
                        _mongoDb.GetCollection<SlackUser>("SlackUsers").FirstOrDefault(x => x.SlackUserId == data.Get("slackId"));
                    if (user == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.ExpectationFailed);
                    }

                    buyerId = user.EmployeeId;
                }
                _mongoDb.Insert(new Purchase() { Price = ice.Price.ToInt(), Buyer = buyerId, Time = DateTime.UtcNow, IceCreamId = iceCreamId}, "Purchases");
            }
            catch (MongoException e)
            {
                return new { success = false, errorMessage = e.Message };
            }
            return new {success = true, errorMessage = string.Empty, quantity = ice.Quantity};
        }
    }
}
