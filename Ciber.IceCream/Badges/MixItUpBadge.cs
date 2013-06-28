using System;
using System.Linq;
using CiberIs.Models;

namespace CiberIs.Badges
{
    public class MixItUpBadge : BadgeJob
    {
        private readonly IMongoDb _mongoDb;

        public MixItUpBadge(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }

        protected override void AwardBadges()
        {
            var badges = _mongoDb.GetCollection<Badge>(CollectionName).ToList();
            var purchasesGroupedByBuyer =
                _mongoDb.GetCollection<Purchase>("Purchases").ToList().GroupBy(x => x.Buyer).ToList();
            var purchases = (from purchasesBuyer in purchasesGroupedByBuyer
                             where
                                 purchasesBuyer.Where(x => !string.IsNullOrEmpty(x.IceCreamId))
                                               .Select(x => x.IceCreamId)
                                               .Distinct()
                                               .Count() >= 10
                             select purchasesBuyer.First().Buyer);
            BadgeRepository.AddBadges(purchases, badges, _mongoDb, "Mix It Up - 10 forskjellige is");
        }

        protected override TimeSpan Interval
        {
            get { return DefaultTimeSpan; }
        }
    }
}