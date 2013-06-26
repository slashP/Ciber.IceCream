using System;
using System.Linq;
using CiberIs.Models;

namespace CiberIs.Badges
{
    public class MedalsBadge : BadgeJob
    {
        private readonly IMongoDb _mongoDb;

        public MedalsBadge(IMongoDb mongoDb)
        {
            _mongoDb = mongoDb;
        }

        protected override void AwardBadges()
        {
            var badges = _mongoDb.GetCollection<Badge>(CollectionName).ToList();
            var groupBy = _mongoDb.GetCollection<Purchase>("Purchases").ToList().GroupBy(x => x.Buyer).ToList();
            var bronzePurchases = groupBy.Where(x => x.Count() > 10).Select(x => x.First().Buyer).ToList();
            var silverPurchases = groupBy.Where(x => x.Count() > 50).Select(x => x.First().Buyer).ToList();
            var goldPurchases = groupBy.Where(x => x.Count() > 100).Select(x => x.First().Buyer).ToList();
            BadgeRepository.AddBadges(bronzePurchases, badges, _mongoDb, "Bronze");
            BadgeRepository.AddBadges(silverPurchases, badges, _mongoDb, "Silver");
            BadgeRepository.AddBadges(goldPurchases, badges, _mongoDb, "Gold");
        }

        protected override TimeSpan Interval
        {
            get { return DefaultTimeSpan; }
        }
    }
}