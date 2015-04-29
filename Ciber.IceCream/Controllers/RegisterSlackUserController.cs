namespace CiberIs.Controllers
{
    using System.Linq;
    using System.Web.Http;

    using CiberIs.Models;

    using MongoDB.Driver.Builders;

    public class RegisterSlackUserController : ApiController
    {
        private IMongoDb _db;

        public RegisterSlackUserController(IMongoDb mongoDb)
        {
            _db = mongoDb;
        }

        public dynamic Post(string slackId, int employeeId)
        {
            var existingUser = _db.GetCollection<SlackUser>("SlackUsers").FirstOrDefault(x => x.SlackUserId == slackId);
            if (existingUser != null)
            {
                MongoHqConfig.RetrieveMongoHqDb().GetCollection<SlackUser>("SlackUsers").Remove(Query.EQ("_id", existingUser.Id));
            }

            _db.Insert(new SlackUser { SlackUserId = slackId, EmployeeId = employeeId }, "SlackUsers");
            return new{success = true};
        }
    }
}