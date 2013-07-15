using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CiberIs.Models;

namespace CiberIs.Controllers
{
    public class BadgeController : ApiController
    {
        private IMongoDb _db;

        public BadgeController(IMongoDb mongoDb)
        {
            _db = mongoDb;
        }

        public Badge Get(int buyer)
        {
            var badge = _db.GetCollection<Badge>("Badges").FirstOrDefault(x => x.Ansattnummer == buyer);
            return badge ?? new Badge();
        }
    }
}
