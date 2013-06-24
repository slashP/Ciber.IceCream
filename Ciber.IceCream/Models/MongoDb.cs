using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace CiberIs.Models
{
    public class MongoDb : IMongoDb
    {
        public IQueryable<T> GetCollection<T>(string collectionName)
        {
            return MongoHqConfig.RetrieveMongoHqDb().GetCollection<T>(collectionName).AsQueryable();
        }

        public void Insert<T>(T entity, string collectionName)
        {
            MongoHqConfig.RetrieveMongoHqDb().GetCollection<T>(collectionName).Insert(entity);
        }

        public T FindById<T>(string objectId, string collectionName)
        {
            return MongoHqConfig.RetrieveMongoHqDb().GetCollection<T>(collectionName).FindOneById(new ObjectId(objectId));
        }

        public void Save<T>(T entity, string collectionName)
        {
            MongoHqConfig.RetrieveMongoHqDb().GetCollection<T>(collectionName).Save(entity);
        }
    }
}