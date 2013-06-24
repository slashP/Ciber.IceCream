using System.Linq;

namespace CiberIs.Models
{
    public interface IMongoDb
    {
        IQueryable<T> GetCollection<T>(string collectionName);
        void Insert<T>(T entity, string collectionName);
        T FindById<T>(string objectId, string collectionName);
        void Save<T>(T entity, string collectionName);
    }
}