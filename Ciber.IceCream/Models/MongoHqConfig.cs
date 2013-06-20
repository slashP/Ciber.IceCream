using System.Configuration;
using MongoDB.Driver;
using System.Linq;

namespace CiberIs.Models
{
    public static class MongoHqConfig
    {
        public static MongoDatabase RetrieveMongoHqDb()
        {
            var connectionstring = ConfigurationManager.AppSettings.Get("MONGOHQ_URL");
            var url = new MongoUrl(connectionstring);
            var client = new MongoClient(url);
            var server = client.GetServer();

            return server.GetDatabase(url.DatabaseName);
        }
    }
}