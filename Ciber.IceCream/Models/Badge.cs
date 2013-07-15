using System.Collections.Generic;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace CiberIs.Models
{
    public class Badge
    {
        [JsonIgnore]
        public BsonObjectId Id { get; set; }
        public int Ansattnummer { get; set; }
        public IList<string> BadgesForUser { get; set; }
    }
}