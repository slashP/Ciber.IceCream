namespace CiberIs.Models
{
    using MongoDB.Bson;

    using Newtonsoft.Json;

    public class SlackUser
    {
        [JsonIgnore]
        public BsonObjectId Id { get; set; }

        public string SlackUserId { get; set; }

        public int EmployeeId { get; set; }
    }
}