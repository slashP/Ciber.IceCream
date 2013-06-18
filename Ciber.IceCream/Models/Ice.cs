using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace CiberIs.Models
{
    public class Ice
    {
        [NotMapped]
        public BsonObjectId Id { get; set; }
        public int Price { get; set; }
        public DateTime? PurchasedTime { get; set; }
        public string PurchasedBy { get; set; }
        public bool IsPaidFor { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
    }
}