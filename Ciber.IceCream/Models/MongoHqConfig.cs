using System.Configuration;
using MongoDB.Driver;

namespace CiberIs.Models
{
    public static class MongoHqConfig
    {
        public static MongoDatabase RetrieveMongoHqDb()
        {
            return new MongoClient(ConfigurationManager.ConnectionStrings["MONGOHQ_URL"].ConnectionString).GetServer().GetDatabase("8e7921a9_3499_4c0e_813e_934605eabbf2");
        }
    }
}