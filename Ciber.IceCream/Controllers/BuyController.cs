namespace CiberIs.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using CiberIs.Badges;
    using CiberIs.Extensions;
    using CiberIs.Models;
    using MongoDB.Driver;

    public class BuyController : ApiController
    {
        readonly IMongoDb _mongoDb;

        readonly IBadgeService _badgeService;

        public BuyController(IMongoDb mongoDb, IBadgeService badgeService)
        {
            _mongoDb = mongoDb;
            _badgeService = badgeService;
        }

        public dynamic Post(FormDataCollection data)
        {
            var iceCreamId = data.Get("iceCreamId");
            var ice = _mongoDb.FindById<IceCream>(iceCreamId, "IceCreams");
            if (ice == null)
            {
                throw new HttpResponseException(HttpStatusCode.ExpectationFailed);
            }

            IEnumerable<string> newBadges;
            try
            {
                ice.Quantity--;
                if (ice.Quantity < 0)
                {
                    throw new HttpResponseException(HttpStatusCode.Conflict);
                }

                _mongoDb.Save(ice, "IceCreams");
                int employeeId;
                if (!int.TryParse(data.Get("buyer"), out employeeId))
                {
                    var user =
                        _mongoDb.GetCollection<SlackUser>("SlackUsers")
                                .FirstOrDefault(x => x.SlackUserId == data.Get("slackId"));
                    if (user == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.ExpectationFailed);
                    }

                    employeeId = user.EmployeeId;
                }

                _mongoDb.Insert(
                    new Purchase
                    {
                        Price = ice.Price.ToInt(), 
                        Buyer = employeeId, 
                        Time = DateTime.UtcNow, 
                        IceCreamId = iceCreamId
                    }, 
                    "Purchases");
                newBadges = _badgeService.UpdateBadgeForEmployee(employeeId);
            }
            catch (MongoException e)
            {
                return new
                {
                    success = false, 
                    errorMessage = e.Message
                };
            }

            return new
            {
                success = true, 
                errorMessage = string.Empty, 
                quantity = ice.Quantity,
                newBadges = newBadges
            };
        }
    }
}