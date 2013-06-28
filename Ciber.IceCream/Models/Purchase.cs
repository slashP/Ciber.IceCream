using System;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;

namespace CiberIs.Models
{
    public class Purchase
    {
        [NotMapped]
        public BsonObjectId Id { get; set; }
        public int Price { get; set; }
        public DateTime? Time { get; set; }
        public int Buyer { get; set; }
        public bool IsPaidFor { get; set; }
        public string IceCreamId { get; set; }
    }
}