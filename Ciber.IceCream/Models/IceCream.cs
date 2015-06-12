using System;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;

namespace CiberIs.Models
{
    public class IceCream
    {
        [NotMapped]
        public BsonObjectId Id { get; set; }
        public double Price { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        [Obsolete("Removed loss functionality. Here for backwards compatibility")]
        public int? Loss { get; set; }
    }
}