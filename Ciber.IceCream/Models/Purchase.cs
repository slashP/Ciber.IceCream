using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using MongoDB.Bson;

namespace CiberIs.Models
{
    public class Purchase
    {
        [NotMapped]
        public BsonObjectId Id { get; set; }
        public int Price { get; set; }
        public DateTime? Time { get; set; }
        public string Name { get; set; }
    }

    public class IceModel
    {
        public IList<Purchase> Purchases { get; set; }
    }
}