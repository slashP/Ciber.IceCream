using System;
using System.Collections.Generic;
using System.Linq;
using CiberIs.Models;

namespace CiberIs.Badges
{
    public class OneFavouriteBadge : BadgeJob
    {
        private readonly IMongoDb _mongoDb;

        public OneFavouriteBadge(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }

        protected override void AwardBadges()
        {
            var badges = _mongoDb.GetCollection<Badge>(CollectionName).ToList();
            var purchasesGroupedByBuyer = _mongoDb.GetCollection<Purchase>("Purchases").ToList().GroupBy(x => x.Buyer).ToList();
            var purchases = new List<int>();
            foreach (var purchasesBuyer in purchasesGroupedByBuyer)
            {
                var longest = 0;
                var temp = 1;
                var lastIteration = purchasesBuyer.First();
                foreach (var purchase in purchasesBuyer.OrderBy(x => x.Time).Where(x => !string.IsNullOrEmpty(x.IceCreamId)).Skip(1))
                {
                    var sameBuy = lastIteration.IceCreamId == purchase.IceCreamId ? 1 : 0;
                    temp = (temp + sameBuy)*sameBuy;
                    longest = Math.Max(temp, longest);
                    lastIteration = purchase;
                }
                if(longest >= 5)
                    purchases.Add(purchasesBuyer.First().Buyer);
            }
            BadgeRepository.AddBadges(purchases, badges, _mongoDb, "Trofast - samme is fem ganger på rad");
        }

        protected override TimeSpan Interval
        {
            get { return DefaultTimeSpan; }
        }
    }
}