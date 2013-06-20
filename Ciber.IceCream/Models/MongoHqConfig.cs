using System.Configuration;
using MongoDB.Driver;
using System.Linq;

namespace CiberIs.Models
{
    public static class MongoHqConfig
    {
        public static MongoDatabase RetrieveMongoHqDb()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MONGOHQ_URL"].ConnectionString;
            var databaseName = connectionString.Split('/').Last();
            return new MongoClient(connectionString).GetServer().GetDatabase(databaseName);
        }
    }
}