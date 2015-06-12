namespace CiberIs.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using CiberIs.Models;

    public class FavouritesController : ApiController
    {
        readonly IMongoDb _mongoDb;

        public FavouritesController(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }

        public dynamic Get(string slackId)
        {
            var existingUser =
                _mongoDb.GetCollection<SlackUser>("SlackUsers")
                   .FirstOrDefault(x => x.SlackUserId == slackId);
            if (existingUser == null)
            {
                throw new NotSupportedException(string.Format("User {0} not found", slackId));
            }

            var employeeId = existingUser.EmployeeId;
            var purchases =
                _mongoDb.GetCollection<Purchase>("Purchases")
                        .Where(x => x.Buyer == employeeId)
                        .ToList();

            var iceCreams = _mongoDb.GetCollection<IceCream>("IceCreams").Select(
                x => new
                {
                    x.Title,
                    Id = x.Id.ToString()
                }).ToDictionary(x => x.Id, x => x.Title);
            var favourites =
                purchases.GroupBy(x => x.IceCreamId)
                         .OrderByDescending(x => x.Count())
                         .Take(3)
                         .Select(
                             x => new
                             {
                                 Name = iceCreams[x.First().IceCreamId],
                                 Quantity = x.Count()
                             });
            return favourites;
        }
    }
}