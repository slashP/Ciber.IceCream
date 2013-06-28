using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace CiberIs.Models
{
    public class Badge
    {
        public BsonObjectId Id { get; set; }
        public int Ansattnummer { get; set; }
        public IList<string> BadgesForUser { get; set; }
    }
}