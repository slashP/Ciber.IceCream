namespace CiberIs.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using CiberIs.Models;

    public class ToplistController : ApiController
    {
        readonly IMongoDb _mongoDb;

        public ToplistController(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }

        public IEnumerable<Rank> Get()
        {
            var purchases =
                _mongoDb.GetCollection<Purchase>("Purchases")
                        .Where(x => x.IceCreamId != null)
                        .ToList();
            var slackUsers = _mongoDb.GetCollection<SlackUser>("SlackUsers").ToList();

            return purchases.GroupBy(x => x.Buyer).OrderByDescending(x => x.Count()).Select(
                x =>
                {
                    var slackUser = slackUsers.FirstOrDefault(u => u.EmployeeId == x.First().Buyer);
                    return new Rank
                    {
                        Quantity = x.Count(),
                        EmployeeId = x.First().Buyer,
                        SlackId = slackUser != null
                            ? slackUser.SlackUserId
                            : null
                    };
                }).Where(x => x.SlackId != null).Take(10);
        }

        public class Rank
        {
            public int Quantity { get; set; }

            public int EmployeeId { get; set; }

            public string SlackId { get; set; }
        }
    }
}